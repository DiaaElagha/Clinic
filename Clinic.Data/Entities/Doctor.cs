using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Data.Entities
{
    public class Doctor : BaseEntity
    {
        [Key]
        public string DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public AppUser DoctorItem { get; set; }

        public Doctor(string CreatedBy = null) : base(CreatedBy)
        {
        }
    }
}
