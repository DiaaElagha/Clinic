using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Constant
{
    public static class SystemConstant
    {
        public static class SeedConst
        {
            public static readonly string USER_NAME = "admin";
            public static readonly string USER_ID = new Guid("c8ec217d-8de6-48b8-8437-e061634ed158").ToString();
            public static readonly Dictionary<UserRoles, string> ROLES_IDS =
                new()
                {
                    { UserRoles.Anonymous, new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210").ToString() },
                    { UserRoles.Client, new Guid("95e8a441-5e60-4e7f-9526-6a88a667629b").ToString() },
                    { UserRoles.Employee, new Guid("bbefce24-12d7-4306-9ad1-af2f52b09822").ToString() },
                    { UserRoles.Admin, new Guid("2033c04e-a343-41bb-84d5-34faa93da32e").ToString() },
                    { UserRoles.Secretary, new Guid("c426fce5-43f1-4d0f-86be-6dd6ba7e4733").ToString() }
                };
            public static readonly int SETTING_ID = 1;
        }



    }
}
