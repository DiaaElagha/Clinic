using AutoMapper;
using Clinic.Core.Dtos;
using Clinic.Core.Helper;
using Clinic.Data.Data;
using Clinic.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Services
{
    public class PatientsService : IPatientsService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public PatientsService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<(List<Patient> Patients, int CountItems)> GetReasonCancellations(int skip, int take, string s = null)
        {
            var query = _db.Patients
                .Include(x => x.PatientItem)
                .Where(
                    x => (s == null
                    || x.PatientItem.FullName.Contains(s)
                    || x.PatientItem.Mobile1.Contains(s)
                    || x.PatientItem.Mobile2.Contains(s)))
                .OrderBy(x => x.CreatedAt);
            return (await query.Skip(skip).Take(take).ToListAsync(), await query.CountAsync());
        }

        public async Task<bool> Delete(string id)
        {
            var item = await GetPatient(id);
            item.IsDelete = true;
            _db.Patients.Update(item);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ReasonCancellationDto> Get(string id)
        {
            return _mapper.Map<ReasonCancellationDto>(await GetPatient(id));
        }

        public async Task<Patient> AddPatient(string userId, ReasonCancellationDto model)
        {
            var item = _mapper.Map(model, new Patient(userId));
            await _db.Patients.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<Patient> UpdatePatient(string userId, string id, ReasonCancellationDto model)
        {
            var item = await GetPatient(id);
            _mapper.Map(model, item);
            item.UpdatedBy = userId;
            item.UpdatedAt = DateTime.Now;
            _db.Patients.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }


        private async Task<Patient> GetPatient(string id)
        {
            var item = await _db.Patients.SingleOrDefaultAsync(x => x.PatientId.Equals(id));
            if (item is null)
                throw new NotFoundException(id);
            return item;
        }
    }
    public interface IPatientsService
    {

    }
}
