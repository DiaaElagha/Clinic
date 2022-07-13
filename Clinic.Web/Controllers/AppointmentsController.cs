using AutoMapper;
using Clinic.Core.Entities;
using Clinic.Infrastructure.Services;
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
    public class AppointmentsController : BaseController
    {
        private IAppointmentsService appointmentsService;
        public AppointmentsController(UserManager<AppUser> systemUsers, 
            IMapper mapper, 
            IAppointmentsService appointmentsService) : base(systemUsers, mapper)
        {
            this.appointmentsService = appointmentsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await appointmentsService.GetAppointments());
        }

    }
}
