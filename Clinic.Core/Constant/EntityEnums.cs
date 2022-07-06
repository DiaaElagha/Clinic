using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Constant
{
    public enum Gender { Male = 0, Female = 1 }
    public enum UserRoles
    {
        Anonymous = 0, 
        Client = 1,
        Employee = 2,
        Admin = 2,
        Secretary = 3,
    }
}
