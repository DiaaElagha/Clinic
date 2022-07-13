using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Models.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "اسم المستخدم")]
        [Required(ErrorMessage = "يرجى ادخال اسم المستخدم")]
        public string Username { get; set; }

        [Display(Name = "كلمة المرور")]
        [Required(ErrorMessage = "يرجى ادخال كلمة المرور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تذكرني؟")]
        public bool RememberMe { get; set; }
    }
}
