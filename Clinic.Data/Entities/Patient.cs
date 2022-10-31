using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Data.Entities
{
    public class Patient : BaseEntity
    {
        [Key]
        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public AppUser PatientItem { get; set; }

        public Patient(string CreatedBy = null) : base(CreatedBy)
        {
        }
    }
}
