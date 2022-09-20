using AutoMapper;
using Clinic.Core.Entities;
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
        private IAppointmentsService _appointmentsService;
        public AppointmentTypesController(
            UserManager<AppUser> systemUsers,
            IMapper mapper,
            IAppointmentsService appointmentsService) : base(systemUsers, mapper)
        {
            _appointmentsService = appointmentsService;
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

            var resultItems = await _appointmentsService.GetAppointmentsTypes(d.Start, d.Length, d.SearchKey);

            var itemsResult = resultItems.AppointmentTypes.Select(x => new
            {
                x.Id,
                x.TypeName,
                x.Note,
                CreateDate = x.CreatedAt.ToString("MM/dd/yyyy")
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
                var item = await _appointmentsService.Get(id.Value);
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
            if (id != null)
            {
                var item = await _appointmentsService.Get(id.Value);
                if (item is not null)
                {
                    _mapper.Map(model, item);
                    await _appointmentsService.UpdateAppointmentType(UserId, item);
                    return Content(ShowMessage.EditSuccessResult(), "application/json");
                }
            }
            else
            {
                var objNew = _mapper.Map<AppointmentType>(model);
                await _appointmentsService.AddAppointmentType(UserId, objNew);
                return Content(ShowMessage.AddSuccessResult(), "application/json");
            }
            return Content(ShowMessage.AddSuccessResult(), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _appointmentsService.Get(id);
            if (item is null)
                return Content(ShowMessage.NotExistResult(), "application/json");
            if (await _appointmentsService.Delete(item))
                return Content(ShowMessage.DeleteSuccessResult(), "application/json");
            return Content(ShowMessage.FailedResult(), "application/json");
        }

    }
}
