using Clinic.Core.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Data.Entities
{
    public class Appointment : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public bool IsShow { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string Note { get; set; }
        public string PatientNote { get; set; }
        public AppointmentStatus Status { get; set; }

        public int? AppointmentTypeId { get; set; }
        [ForeignKey(nameof(AppointmentTypeId))]
        public AppointmentType AppointmentType { get; set; }

        public int? ReasonCancellationId { get; set; }
        [ForeignKey(nameof(ReasonCancellationId))]
        public ReasonCancellation ReasonCancellation { get; set; }

        public string PatientBy { get; set; }
        [ForeignKey(nameof(PatientBy))]
        public AppUser PatientByItem { get; set; }

        public string DoctorBy { get; set; }
        [ForeignKey(nameof(DoctorBy))]
        public AppUser DoctorByItem { get; set; }

        public Appointment(string CreatedBy = null) : base(CreatedBy)
        {
            IsShow = true;
            Status = AppointmentStatus.Pending;
        }
    }
}
