using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Models.ViewModels
{
    public class AppointmentTypeVM
    {
        [Display(Name = "اسم النوع")]
        [Required(ErrorMessage = "يرجى ادخال النوع")]
        public string TypeName { get; set; }

        [Display(Name = "الملاحظات")]
        public string Note { get; set; }

        [Display(Name = "السماح بالحجز")]
        public bool AllowAppointment { get; set; }
    }
}
