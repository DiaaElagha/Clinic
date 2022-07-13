using AutoMapper;
using Clinic.Core.Constant;
using Clinic.Core.Entities;
using Clinic.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Clinic.Web.Controllers
{
    //[Authorize]
    public class BaseController : Controller
    {
        protected IMapper _mapper;
        protected readonly UserManager<AppUser> _systemUsers;
        protected String UserId;
        protected String UserName;
        protected UserRoles? UserRole;
        protected AppUser CurrentUser = null;
        public BaseController(UserManager<AppUser> systemUsers, IMapper mapper)
        {
            _systemUsers = systemUsers;
            _mapper = mapper;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                base.OnActionExecuting(filterContext);

                UserId = _systemUsers.GetUserId(HttpContext.User);
                CurrentUser = HttpContext.Session.GetObject<AppUser>("CurrentUser");
                if (CurrentUser == null)
                {
                    SetValuesInSession(UserId);
                }

                UserName = CurrentUser.UserName;
                UserRole = CurrentUser.Role;
                ViewBag.UserId = UserId;
                ViewBag.FullName = CurrentUser.FullName;
            }
            void SetValuesInSession(string userId)
            {
                CurrentUser = _systemUsers.Users.SingleOrDefault(x => x.Id.Equals(UserId));
                HttpContext.Session.SetObject("CurrentUser", CurrentUser);
            }
        }

        public AppUser GetUser()
        {
            var user = _systemUsers.Users.SingleOrDefault(x => x.Id.Equals(UserId));
            return user;
        }

        public static string GetIpAddress()  // Get IP Address
        {
            string ip = "";
            IPHostEntry ipEntry = Dns.GetHostEntry(GetCompCode());
            IPAddress[] addr = ipEntry.AddressList;
            ip = addr[2].ToString();
            return ip;
        }

        public static string GetCompCode()  // Get Computer Name
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();
            return strHostName;
        }

        public static string GetMacAddress()  // Get Mac Address (Physical Address)
        {
            String firstMacAddress = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();
            return firstMacAddress;
        }

    }
}
