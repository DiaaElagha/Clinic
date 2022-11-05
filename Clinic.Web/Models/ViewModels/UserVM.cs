using Clinic.Core.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Models.ViewModels
{
    public class UserVM
    {
        [Display(Name = "اسم المستخدم")]
        [Required(ErrorMessage = "يرجى ادخال الاسم")]
        public string UserName { get; set; }

        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Display(Name = "الاسم كامل")]
        [Required(ErrorMessage = "يرجى ادخال الاسم")]
        public string FullName { get; set; }

        [Display(Name = "فعالية المستخدم")]
        [Required(ErrorMessage = "يرجى ادخال الفعالية")]
        public bool IsActive { get; set; }
    }
}
