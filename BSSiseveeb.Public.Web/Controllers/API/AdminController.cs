using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    [AuthorizeLevel5]
    public class AdminController : ApiController
    {
        public IVacationRepository VacationRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }

        [HttpGet]
        public IEnumerable<Vacation> GetPendingVacations()
        {
            return VacationRepository.Where(x => x.Status == VacationStatus.Pending).ToList();
        }

        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            return EmployeeRepository.ToList();
        }

        [HttpGet]
        public int ApproveVacation(int id)
        {
            var vacation = VacationRepository.First(x => x.Id == id);
            vacation.Status = VacationStatus.Approved;
            VacationRepository.SaveOrUpdate(vacation);
            VacationRepository.Commit();
            return 0;
        }

        [HttpGet]
        public int DeclineVacation(int id)
        {
            var vacation = VacationRepository.First(x => x.Id == id);
            vacation.Status = VacationStatus.Declined;
            VacationRepository.SaveOrUpdate(vacation);
            VacationRepository.Commit();
            return 0;
        }
    }
}