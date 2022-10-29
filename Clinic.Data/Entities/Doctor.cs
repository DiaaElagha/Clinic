using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
