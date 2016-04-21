using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;

namespace BSSiseveeb.Public.Web.Controllers.API
{

    public class WorkersController : ApiController
    {

        public IEmployeeRepository EmployeeRepository { get; set; }

        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            return EmployeeRepository.ToList();
        }
    }
}