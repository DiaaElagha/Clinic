using Clinic.Core.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web.Models.ViewModels
{
    public class UserVM
    {
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public UserRoles? Role { get; set; }
    }
}
