using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;




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
            var currentUser =
                HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(User.Identity.GetUserId())
                    .EmployeeId;

            int days = (int)model.End.Subtract(model.Start).TotalDays + 1;

            if (model.Start > model.End)
            {
                return BadRequest();
            }
            
            VacationRepository.AddIfNew(new Vacation()
            {
                StartDate = model.Start,
                EndDate = model.End,
                Status = VacationStatus.Pending,
                EmployeeId = currentUser,
                Days = days,
            });
            VacationRepository.Commit();

            return Ok();
        }
    }
}