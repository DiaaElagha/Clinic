using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Dtos
{
    public class AppointmentTypeDto
    {
        public string TypeName { get; set; }
        public string Note { get; set; }
        public bool AllowAppointment { get; set; }
    }
}
