using AutoMapper;
using Clinic.Core.Entities;
using Clinic.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Helper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<AppointmentTypeVM, AppointmentType>().ReverseMap();
        }
    }
}
