using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Contracts.Services;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;
using BSSiseveeb.Core.Mappers;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace BSSiseveeb.Public.Web.Controllers
{
    public abstract partial class BaseController : Controller
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IVacationRepository VacationRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public IADService ADService { get; set; }
        public ICsvImportService CsvImportService { get; set; }

        public string CurrentUserId
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return null;
                }

                var identity = (ClaimsIdentity)ClaimsPrincipal.Current.Identity;
                return identity.FindFirst(AppClaims.ObjectIdentifier).Value;
            }
        }

        public Employee CurrentUser
        {
            get
            {
                var user = EmployeeRepository.FirstOrDefault(x => x.Id == CurrentUserId);
                if (user == null)
                {
                    user = new Employee()
                    {
                        Role = new Role() { Rights = AccessRights.None }
                    };
                }

                return user;
            }
        }

        public RoleDto CurrentUserRole
        {
            get
            {
                return CurrentUser.Role.AsDto();
            }
        }
    }
}