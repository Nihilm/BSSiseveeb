using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;
using BSSiseveeb.Core.Mappers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace BSSiseveeb.Public.Web.Controllers.API
{
    public abstract class BaseApiController : ApiController
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public IVacationRepository VacationRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }

        [HttpGet]
        public IEnumerable<EmployeeDto> GetEmployees()
        {
            return EmployeeRepository.AsDto();
        }

        public string CurrentUserId => User.Identity.GetUserId();

        public ApplicationUser CurrentUser
        {
            get
            {
                return HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(CurrentUserId);
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set { _roleManager = value; }
        }
    }
}