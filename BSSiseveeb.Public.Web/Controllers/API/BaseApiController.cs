using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace BSSiseveeb.Public.Web.Controllers.API
{
    public class BaseApiController : ApiController
    {
        public IVacationRepository VacationRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }

        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            return EmployeeRepository.ToList();
        }

        public ApplicationUser CurrentUser()
        {
            return HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(User.Identity.GetUserId());
        }
    }
}