using AutoMapper;
using Clinic.Data.Entities;
using Clinic.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<AppUser> systemUsers, IMapper mapper) : base(systemUsers, mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
