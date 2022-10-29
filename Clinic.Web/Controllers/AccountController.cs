using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Data.Entities;
using Clinic.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> systemUsers,
            IMapper mapper,
            SignInManager<AppUser> signInManager) : base(systemUsers, mapper)
        {
            _signInManager = signInManager;
        }

        [Route("/Login")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Route("/Login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);
            var user = await _systemUsers.Users.SingleOrDefaultAsync(x => x.UserName.Equals(model.Username));
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "محاولة تسجيل الدخول غير صالحة.");
                return View(model);
            }
            if (!user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "المستخدم غير فعال.");
                return View(model);
            }
            if (user.ExpiryDate.HasValue)
            {
                if (user.ExpiryDate.Value.Date < DateTime.Now.Date)
                {
                    ModelState.AddModelError(string.Empty, "المستخدم منتهي الصلاحية.");
                    return View(model);
                }
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded)
            {
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                await _signInManager.SignInWithClaimsAsync(user,
                    new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.Now.AddYears(1)
                    }, claims);

                if (returnUrl != null)
                    return Redirect(returnUrl);
                else
                    return Redirect("/Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "محاولة تسجيل الدخول غير صالحة.");
                return View(model);
            }
        }

        [Route("/Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home");
        }

        [Route("/AccessDenied")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Clinic.Web.Controllers.HomeController.Index), "Home");
            }
        }

    }
}