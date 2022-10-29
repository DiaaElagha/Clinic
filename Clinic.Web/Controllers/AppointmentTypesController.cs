using AutoMapper;
using Clinic.Core.Constant;
using Clinic.Core.Dtos;
using Clinic.Data.Entities;
using Clinic.Infrastructure.Services;
using Clinic.Web.Helper;
using Clinic.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Controllers
{
    public class AppointmentTypesController : BaseController
    {
        private IAppointmentTypeService _appointmentTypeService;
        public AppointmentTypesController(
            UserManager<AppUser> systemUsers,
            IMapper mapper,
            IAppointmentTypeService appointmentTypeService) : base(systemUsers, mapper)
        {
            _appointmentTypeService = appointmentTypeService;
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

            var resultItems = await _appointmentTypeService.GetAppointmentsTypes(d.Start, d.Length, d.SearchKey);

            var itemsResult = resultItems.AppointmentTypes.Select(x => new
            {
                x.Id,
                x.TypeName,
                x.Note,
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
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            if (id != null)
            {
                var item = await _appointmentTypeService.Get(id.Value);
                if (item is not null)
                {
                    var model = _mapper.Map<AppointmentTypeVM>(item);
                    return View(model);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(int? id, AppointmentTypeVM model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id is not null)
            {
                await _appointmentTypeService.UpdateAppointmentType(UserId, id.Value, _mapper.Map<AppointmentTypeDto>(model));
                return Ok(ResultsMessage.EditSuccessResult());
            }
            else
            {
                await _appointmentTypeService.AddAppointmentType(UserId, _mapper.Map<AppointmentTypeDto>(model));
                return Ok(ResultsMessage.AddSuccessResult());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _appointmentTypeService.Delete(id))
                return Ok(ResultsMessage.DeleteSuccessResult());
            return Ok(ResultsMessage.FailedResult());
        }

    }
}
