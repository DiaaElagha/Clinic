using Clinic.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Entities
{
    public class AppointmentType : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Note { get; set; }
        public bool AllowAppointment { get; set; }

        public AppointmentType(string CreatedBy = null) : base(CreatedBy)
        {
            AllowAppointment = true;
        }
    }
}
