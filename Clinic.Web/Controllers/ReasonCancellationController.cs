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
    public class ReasonCancellationController : BaseController
    {
        private IReasonCancellationService _reasonCancellationService;
        public ReasonCancellationController(
            UserManager<AppUser> systemUsers,
            IMapper mapper,
            IReasonCancellationService reasonCancellationService) : base(systemUsers, mapper)
        {
            _reasonCancellationService = reasonCancellationService;
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

            var resultItems = await _reasonCancellationService.GetReasonCancellations(d.Start, d.Length, d.SearchKey);

            var itemsResult = resultItems.ReasonCancellations.Select(x => new
            {
                x.Id,
                x.Title,
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
                var item = await _reasonCancellationService.Get(id.Value);
                if (item is not null)
                {
                    var model = _mapper.Map<ReasonCancellationVM>(item);
                    return View(model);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(int? id, ReasonCancellationVM model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id is not null)
            {
                await _reasonCancellationService.UpdateReasonCancellation(UserId, id.Value, _mapper.Map<ReasonCancellationDto>(model));
                return Ok(ResultsMessage.EditSuccessResult());
            }
            else
            {
                await _reasonCancellationService.AddReasonCancellation(UserId, _mapper.Map<ReasonCancellationDto>(model));
                return Ok(ResultsMessage.AddSuccessResult());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _reasonCancellationService.Delete(id))
                return Ok(ResultsMessage.DeleteSuccessResult());
            return Ok(ResultsMessage.FailedResult());
        }

    }
}
