using Clinic.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Appointment> Appointments { set; get; }
        public DbSet<AppointmentType> AppointmentTypes { set; get; }
        public DbSet<Doctor> Doctors { set; get; }
        public DbSet<Patient> Patients { set; get; }
        public DbSet<ReasonCancellation> ReasonsCancellation { set; get; }

        public DbSet<Attachment> Attachments { set; get; }
        public DbSet<ContactUs> ContactUs { set; get; }
        public DbSet<ExternalRequest> ExternalRequests { set; get; }
        public DbSet<GeneralSettings> GeneralSettings { set; get; }
    }
}
