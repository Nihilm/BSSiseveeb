using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;



namespace BSSiseveeb.Public.Web.Controllers.Api
{
    [AuthorizeLevel1]
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

        [HttpGet]
        public int SetVacation(DateTime start, DateTime end)
        {
            var currentUser =
                HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(User.Identity.GetUserId())
                    .EmployeeId;

            int days = (int)end.Subtract(start).TotalDays;

            if (start > end)
            {
                return 1;
            }
            
            VacationRepository.AddIfNew(new Vacation()
            {
                StartDate = start,
                EndDate = end,
                Status = VacationStatus.Pending,
                EmployeeId = currentUser,
                Days = days,
            });
            VacationRepository.Commit();
            return 0;
        }
    }
}