using AutoMapper;
using Clinic.Data.Entities;
using Clinic.Infrastructure.Services;
using Clinic.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Web.Controllers
{
    public class AppointmentsController : BaseController
    {
        public AppointmentsController(UserManager<AppUser> systemUsers, 
            IMapper mapper) : base(systemUsers, mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

    }
}
