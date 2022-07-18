using Clinic.Core.Entities;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Services
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly ApplicationDbContext _db;
        public AppointmentsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Appointment>> GetAppointments()
        {
            return await _db.Appointments.ToListAsync();
        }

        public async Task<(List<AppointmentType> AppointmentTypes, int CountItems)> GetAppointmentsTypes(int skip, int take, string s = null)
        {
            var listItems = _db.AppointmentTypes
                .Where(
                    x => (s == null
                    || x.TypeName.Contains(s)
                    || x.Note.Contains(s)))
                .OrderBy(x => x.CreatedAt);
            return (await listItems.Skip(skip).Take(take).ToListAsync(), await listItems.CountAsync());
        }

        public async Task<bool> Delete(AppointmentType item)
        {
            _db.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<AppointmentType> Get(int id)
        {
            var item = await _db.AppointmentTypes.SingleOrDefaultAsync(x => x.Id == id);
            return item;
        }
    }
    public interface IAppointmentsService
    {
        Task<bool> Delete(AppointmentType item);
        Task<AppointmentType> Get(int id);
        Task<List<Appointment>> GetAppointments();
        Task<(List<AppointmentType> AppointmentTypes, int CountItems)> GetAppointmentsTypes(int skip, int take, string s = null);
    }
}
