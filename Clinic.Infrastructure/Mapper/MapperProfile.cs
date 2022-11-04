using AutoMapper;
using Clinic.Core.Dtos;
using Clinic.Data.Entities;

namespace Clinic.Infrastructure.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AppointmentTypeDto, AppointmentType>().ReverseMap();
            CreateMap<ReasonCancellationDto, ReasonCancellation>().ReverseMap();
        }
    }
}
