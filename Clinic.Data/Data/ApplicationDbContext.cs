using Clinic.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, RoleUser, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<AppointmentType>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<Doctor>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<Patient>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<ReasonCancellation>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<ContactUs>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<ExternalRequest>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<GeneralSettings>().HasQueryFilter(x => !x.IsDelete);


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
