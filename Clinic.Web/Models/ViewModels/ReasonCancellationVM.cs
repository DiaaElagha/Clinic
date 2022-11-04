using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Models.ViewModels
{
    public class ReasonCancellationVM
    {
        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "يرجى ادخال العنوان")]
        public string Title { get; set; }

        [Display(Name = "الملاحظات")]
        public string Note { get; set; }
    }
}
