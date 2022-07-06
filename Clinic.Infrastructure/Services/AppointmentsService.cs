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

    }
    public interface IAppointmentsService
    {
        Task<List<Appointment>> GetAppointments();
    }
}
