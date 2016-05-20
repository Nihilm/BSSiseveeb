using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Contracts.Services;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;
using BSSiseveeb.Core.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    public abstract class BaseApiController : ApiController
    {
        public IVacationRepository VacationRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public IEmailService EmailService { get; set; }

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

        protected IEnumerable<EmployeeDto> GetEmployees()
        {
            return EmployeeRepository.AsDto();
        }
    }
}