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
        [Required(ErrorMessage = "يرجى ادخال الاسم")]
        public string TypeName { get; set; }
    }
}
