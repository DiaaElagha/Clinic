using AutoMapper;
using Clinic.Core.Dtos;
using Clinic.Web.Models.ViewModels;

namespace Clinic.Web.Helper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<AppointmentTypeVM, AppointmentTypeDto>().ReverseMap();
            CreateMap<ReasonCancellationVM, ReasonCancellationDto>().ReverseMap();
        }
    }
}
