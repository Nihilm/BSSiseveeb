using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    [AuthorizeLevel(AccessRights.Level1)]
    public class CalendarController : BaseApiController
    {
        [HttpGet]
        public IEnumerable<Vacation> GetVacations (DateTime date)
        {
            var startRange = new DateTime(date.Year, date.Month, 1);
            var endRange = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
            return VacationRepository.Where(x => x.StartDate <= endRange 
                    && startRange <= x.EndDate
                    && x.Status == VacationStatus.Approved)
                    .ToList();
        }

        [HttpPost]
        public IHttpActionResult SetVacation(ApiControllerModels.VacationModel model)
        {
            var currentUser = CurrentUser.EmployeeId;

            int days = (int)model.End.Subtract(model.Start).TotalDays + 1;

            if (model.Start > model.End)
            {
                return BadRequest();
            }

            var employee = EmployeeRepository.First(x => x.Id == currentUser);

            if (employee.VacationDays - days < 0)
            {
                return BadRequest();
            }

            employee.VacationDays -= days;
            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();


            //TODO: Lisainfo väljale piirangud

            VacationRepository.AddIfNew(new Vacation()
            {
                StartDate = model.Start,
                EndDate = model.End,
                Status = VacationStatus.Pending,
                EmployeeId = currentUser,
                Days = days,
                Comments = model.Comment
            });
            VacationRepository.Commit();

            return Ok();
        }

        [HttpGet]
        public List<Vacation> GetMyVacation()
        {
            var result = VacationRepository.Where(x => x.EmployeeId == CurrentUser.EmployeeId)
                .Where(x => x.Status == VacationStatus.Approved || x.Status == VacationStatus.Pending).ToList();
            return result;
        }

        [HttpPost]
        public IHttpActionResult CancelVacation(ApiControllerModels.GeneralIdModel model)
        {
            var vacation = VacationRepository.First(x => x.Id == model.Id);
            var currentUser = CurrentUser.EmployeeId;
            var employee = EmployeeRepository.First(x => x.Id == currentUser);

            employee.VacationDays += vacation.Days;
            vacation.Status = VacationStatus.Declined;

            EmployeeRepository.SaveOrUpdate(employee);
            VacationRepository.SaveOrUpdate(vacation);

            EmployeeRepository.Commit();
            VacationRepository.Commit();
            return Ok();
        } 
    }
}