using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BSSiseveeb.Public.Web.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IVacationRepository VacationRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        public ApplicationUser CurrentUser()
        {
            return HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(User.Identity.GetUserId());
        }
    }
}