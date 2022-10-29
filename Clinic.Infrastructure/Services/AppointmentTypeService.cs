using AutoMapper;
using Clinic.Core.Dtos;
using Clinic.Core.Helper;
using Clinic.Data.Data;
using Clinic.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Services
{
    public class AppointmentTypeService : IAppointmentTypeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public AppointmentTypeService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<(List<AppointmentType> AppointmentTypes, int CountItems)> GetAppointmentsTypes(int skip, int take, string s = null)
        {
            var query = _db.AppointmentTypes
                .Where(
                    x => (s == null
                    || x.TypeName.Contains(s)
                    || x.Note.Contains(s)))
                .OrderBy(x => x.CreatedAt);
            return (await query.Skip(skip).Take(take).ToListAsync(), await query.CountAsync());
        }

        public async Task<bool> Delete(int id)
        {
            var item = await GetAppointmentType(id);
            item.IsDelete = true;
            _db.AppointmentTypes.Update(item);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<AppointmentTypeDto> Get(int id)
        {
            return _mapper.Map<AppointmentTypeDto>(await GetAppointmentType(id));
        }

        public async Task<AppointmentType> AddAppointmentType(string userId, AppointmentTypeDto model)
        {
            var item = _mapper.Map(model, new AppointmentType(userId));
            await _db.AppointmentTypes.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<AppointmentType> UpdateAppointmentType(string userId, int id, AppointmentTypeDto model)
        {
            var item = await GetAppointmentType(id);
            _mapper.Map(model, item);
            item.UpdatedBy = userId;
            item.UpdatedAt = DateTime.Now;
            _db.AppointmentTypes.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }


        private async Task<AppointmentType> GetAppointmentType(int id)
        {
            var item = await _db.AppointmentTypes.SingleOrDefaultAsync(x => x.Id == id);
            if (item is null)
                throw new NotFoundException(id);
            return item;
        }
    }
    public interface IAppointmentTypeService
    {
        Task<AppointmentType> AddAppointmentType(string userId, AppointmentTypeDto model);
        Task<bool> Delete(int id);
        Task<AppointmentTypeDto> Get(int id);
        Task<(List<AppointmentType> AppointmentTypes, int CountItems)> GetAppointmentsTypes(int skip, int take, string s = null);
        Task<AppointmentType> UpdateAppointmentType(string userId, int id, AppointmentTypeDto model);
    }
}
