using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;

namespace BSSiseveeb.Public.Web.Controllers.Api
{
    public class CalendarController : ApiController
    {
        public IVacationRepository VacationRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }

        [HttpGet]
        public IEnumerable<Vacation> GetVacations (DateTime date)
        {
            var startRange = new DateTime(date.Year, date.Month, 1);
            var endRange = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            return VacationRepository.Where(x => x.StartDate <= endRange 
                    && startRange <= x.EndDate
                    && x.Status == VacationStatus.Approved)
                    .ToList();
        }

        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            return EmployeeRepository.ToList();
        }  
    }
}