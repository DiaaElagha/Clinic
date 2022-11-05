using AutoMapper;
using Clinic.Core.Constant;
using Clinic.Core.Dtos;
using Clinic.Data.Entities;
using Clinic.Infrastructure.Services;
using Clinic.Web.Helper;
using Clinic.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PatientsController : BaseController
    {
        private IPatientsService _patientsService;
        public PatientsController(
            UserManager<AppUser> systemUsers,
            IMapper mapper,
            IPatientsService patientsService) : base(systemUsers, mapper)
        {
            _patientsService = patientsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetAll([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var resultItems = await _patientsService.GetPatients(d.Start, d.Length, d.SearchKey);

            var itemsResult = resultItems.Patients.Select(x => new
            {
                x.PatientId,
                FullName = x.PatientItem.FullName,
                CreateDate = x.CreatedAt.ToString(SystemConstant.FormatDate)
            }).ToList();
            var result =
               new
               {
                   draw = d.Draw,
                   recordsTotal = resultItems.CountItems,
                   recordsFiltered = resultItems.CountItems,
                   data = itemsResult
               };
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(string id)
        {
            if (!id.IsNull())
            {
                var item = await _patientsService.Get(id);
                if (item is not null)
                {
                    var model = _mapper.Map<PatientVM>(item);
                    return View(model);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(string id, PatientVM model)
        {
            if (!ModelState.IsValid) return View(model);
            if (!id.IsNull())
            {
                await _patientsService.UpdatePatient(UserId, id, _mapper.Map<PatientDto>(model));
                return Ok(ResultsMessage.EditSuccessResult());
            }
            else
            {
                await _patientsService.AddPatient(UserId, _mapper.Map<PatientDto>(model));
                return Ok(ResultsMessage.AddSuccessResult());
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (await _appointmentTypeService.Delete(id))
        //        return Ok(ResultsMessage.DeleteSuccessResult());
        //    return Ok(ResultsMessage.FailedResult());
        //}


    }
}
