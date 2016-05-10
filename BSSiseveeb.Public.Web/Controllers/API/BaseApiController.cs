using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;
using BSSiseveeb.Core.Mappers;
using System.Linq;
using BSSiseveeb.Core;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    public abstract class BaseApiController : ApiController
    {
        public IVacationRepository VacationRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }

        [Authorize]
        [HttpGet]
        public IEnumerable<EmployeeDto> GetEmployees()
        {
            return EmployeeRepository.AsDto();
        }

        public string CurrentUserId
        {
            get
            {
                var identity = (ClaimsIdentity)ClaimsPrincipal.Current.Identity;
                return identity.FindFirst(AppClaims.ObjectIdentifier).Value;
            }
        }

        public Employee CurrentUser
        {
            get
            {
                return EmployeeRepository.FirstOrDefault(x => x.Id == CurrentUserId);
            }
        }
    }
}