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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(AppointmentTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //ModelState.AddModelError("TypeName","Hi whats up");
            //return View(model);
            return Content(ShowMessage.AddSuccessResult(), "application/json");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(int? id, MaterialVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    if (id != null)
        //    {
        //        var item = await _context.Materials.FindAsync(id.Value);
        //        if (item != null)
        //        {
        //            item.TitleAR = model.TitleAR;
        //            item.TitleEN = model.TitleEN;
        //            item.UpdatedBy = UserId;
        //            item.UpdatedAt = DateTime.Now;
        //            _context.Materials.Update(item);
        //             await _context.SaveChangesAsync();
        //            return Content(ShowMessage.EditSuccessResult(), "application/json");
        //        }
        //    }
        //    else
        //    {
        //        var objNew = _mapper.Map<Material>(model);
        //        objNew.CreatedBy = UserId;
        //        await _context.Materials.AddAsync(objNew);
        //        await _context.SaveChangesAsync();
        //        return Content(ShowMessage.AddSuccessResult(), "application/json");
        //    }
        //    return Content(ShowMessage.FailedResult(), "application/json");
        //}

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
