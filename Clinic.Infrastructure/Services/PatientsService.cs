using AutoMapper;
using Clinic.Core.Constant;
using Clinic.Core.Dtos;
using Clinic.Core.Helper;
using Clinic.Data.Data;
using Clinic.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Services
{
    public class PatientsService : IPatientsService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public PatientsService(ApplicationDbContext db, IMapper mapper, UserManager<AppUser> userManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<(List<Patient> Patients, int CountItems)> GetPatients(int skip, int take, string s = null)
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

        public async Task<PatientDto> Get(string id)
        {
            return _mapper.Map<PatientDto>(await GetPatient(id));
        }

        public async Task<Patient> AddPatient(string userId, PatientDto model)
        {
            var itemUser = _mapper.Map(model.PatientItem, new AppUser(userId));
            itemUser.Role = UserRoles.Client;
            var resultUser = await _userManager.CreateAsync(itemUser, model.PatientItem.Password);
            if (resultUser.Succeeded)
            {
                var item = _mapper.Map(model, new Patient(userId));
                item.PatientItem = null;
                item.PatientId = itemUser.Id;
                await _db.Patients.AddAsync(item);
                await _db.SaveChangesAsync();
                return item;
            }
            return null;
        }

        public async Task<Patient> UpdatePatient(string userId, string id, PatientDto model)
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
            var item = await _db.Patients.Include(x => x.PatientItem).SingleOrDefaultAsync(x => x.PatientId.Equals(id));
            if (item is null)
                throw new NotFoundException(id);
            return item;
        }
    }
    public interface IPatientsService
    {
        Task<Patient> AddPatient(string userId, PatientDto model);
        Task<PatientDto> Get(string id);
        Task<(List<Patient> Patients, int CountItems)> GetPatients(int skip, int take, string s = null);
        Task<Patient> UpdatePatient(string userId, string id, PatientDto model);
    }
}
