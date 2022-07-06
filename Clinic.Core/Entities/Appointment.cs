using Clinic.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class Appointment : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public bool IsActive { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        public string PatientBy { get; set; }
        [ForeignKey(nameof(PatientBy))]
        public AppUser PatientByItem { get; set; }

        public string DoctorBy { get; set; }
        [ForeignKey(nameof(DoctorBy))]
        public AppUser DoctorByItem { get; set; }

        public Appointment(string CreatedBy = null) : base(CreatedBy)
        {
            IsActive = false;
        }
    }
}
