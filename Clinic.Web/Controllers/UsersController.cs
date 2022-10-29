using AutoMapper;
using Clinic.Data.Entities;
using Clinic.Web.Helper;
using Clinic.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(UserManager<AppUser> systemUsers,
            IMapper mapper) : base(systemUsers, mapper)
        {
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

            var rolesList = _systemUsers.Users
                .Where(
                    x => 
                    (d.SearchKey == null
                        || x.UserName.Contains(d.SearchKey)
                        || x.Mobile1.Contains(d.SearchKey)
                        || x.Mobile2.Contains(d.SearchKey)
                        || x.LandPhone.Contains(d.SearchKey)
                        || x.Address.Contains(d.SearchKey)
                        || x.FullName.Contains(d.SearchKey)
                    ))
                .OrderBy(x => x.Id);

            int totalCount = rolesList.Count();

            var items = await rolesList
                .Skip(d.Start)
                .Take(d.Length)
                .ToListAsync();

            var itemsResult = items.Select(x => new
            {
                x.Id,
                x.UserName,
                x.FullName,
                Mobile = x.Mobile1 ?? x.Mobile2,
                x.IsActive,
                RoleType = x.Role.ToString()
            }).ToList();
            var result =
               new
               {
                   draw = d.Draw,
                   recordsTotal = totalCount,
                   recordsFiltered = totalCount,
                   data = itemsResult
               };
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string id)
        {
            if (!id.IsNull())
            {
                var item = await _systemUsers.FindByIdAsync(id);
                if (item != null)
                {
                    var model = _mapper.Map<UserVM>(item);
                    return View(model);
                }
            }
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(string id, UserVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    if (!id.IsNull())
        //    {
        //        var item = await _systemUsers.FindByIdAsync(id);
        //        if (item != null)
        //        {
        //            if (await _systemUsers.Users.AnyAsync(x => x.UserName.Equals(model.UserName) && !x.Id.Equals(id)))
        //            {
        //                return Content(ShowMessage.AddSuccessResult("w: اسم المستخدم موجود مسبفا!"), "application/json");
        //            }

        //            if (item.Role != model.Role)
        //            {
        //                var userRoles = _systemUsers.UserRoles.Where(x => x.UserId.Equals(id));
        //                _systemUsers.UserRoles.RemoveRange(userRoles);
        //                await _systemUsers.UserRoles.AddAsync(new IdentityUserRole<string>
        //                {
        //                    RoleId = ModelBuilderExtensions.SeedConst.ROLES_IDS[model.Role],
        //                    UserId = id
        //                });
        //            }

        //            MergeProperties.CoalesceTo(model, item);
        //            item.IsActive = model.IsActive;
        //            item.Role = model.Role;
        //            item.Gender = model.Gender;
        //            item.PasswordHash = Utilities.HashPassword(model.Password ?? $"A!{Utilities.GetRandomString()}");
        //            _systemUsers.Users.Update(item);

        //            await _systemUsers.SaveChangesAsync();

        //            return Content(ShowMessage.EditSuccessResult(), "application/json");
        //        }
        //    }
        //    else
        //    {
        //        if (await _systemUsers.Users.AnyAsync(x => x.UserName.Equals(model.UserName)))
        //        {
        //            await _systemUsers.SaveChangesAsync();
        //            return Content(ShowMessage.AddSuccessResult("w: اسم المستخدم موجود مسبفا!"), "application/json");
        //        }

        //        var objNew = _mapper.Map<AppUser>(model);
        //        await _systemUsers.Users.AddAsync(objNew);
        //        await _systemUsers.SaveChangesAsync();

        //        await _systemUsers.UserRoles.AddAsync(new IdentityUserRole<string>
        //        {
        //            RoleId = ModelBuilderExtensions.SeedConst.ROLES_IDS[model.Role],
        //            UserId = objNew.Id
        //        });
        //        await _systemUsers.SaveChangesAsync();
        //        return Content(ShowMessage.AddSuccessResult(), "application/json");
        //    }
        //    return Content(ShowMessage.FailedResult(), "application/json");
        //}

        //[HttpGet]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var item = await _systemUsers.Users.FindAsync(id);
        //    _systemUsers.Users.Remove(item);
        //    await _systemUsers.SaveChangesAsync();
        //    return Content(ShowMessage.DeleteSuccessResult(), "application/json");
        //}

    }
}
