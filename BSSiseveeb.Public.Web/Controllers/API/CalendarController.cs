using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Controllers.API.Helpers;
using BSSiseveeb.Public.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    [Authorize]
    public class CalendarController : BaseApiController
    {
        [Authorize]
        [HttpGet]
        [CacheOutput(ServerTimeSpan = 300, ClientTimeSpan = 300)]
        public new IEnumerable<EmployeeDto> GetEmployees()
        {
            return base.GetEmployees();
        }

        [HttpGet]
        [AuthorizeApi(AccessRights.Standard)]
        public IHttpActionResult GetVacations(DateTime date)
        {
            var startRange = new DateTime(date.Year, date.Month, 1);
            var endRange = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);

            return Ok(VacationRepository
                .Where(x =>
                    x.StartDate <= endRange &&
                    startRange <= x.EndDate)
                    .Where(x => 
                        x.Status == VacationStatus.Approved || 
                        x.Status == VacationStatus.Retired)
                .AsDto());
        }

        
        [HttpPost]
        [AuthorizeApi(AccessRights.Standard)]
        public IHttpActionResult SetVacation(VacationModel model)
        {
            var id = CurrentUserId;
            var currentUserVacations = VacationRepository.Where(x => x.EmployeeId == id)
                                .Where(x => x.Status != VacationStatus.Declined || x.Status != VacationStatus.Retired)
                                .ToList();

            int days = (int)model.End.Subtract(model.Start).TotalDays + 1;

            if (model.Start > model.End)
            {
                return BadRequest("ERROR: Puhkuse lõpp on enne puhkuse algust");
            }

            var employee = EmployeeRepository.First(x => x.Id == id);

            if (employee.VacationDays - days < 0)
            {
                return BadRequest("ERROR: Pole piisavalt kasutamata puhkusepäevi");
            }


            if (model.Comment?.Length > 250)
            {
                return BadRequest("ERROR: Lisainfo väljal ei tohi olla rohkem kui 250 tähemärki");
            }


            if (days == 14 || days <= 7)
            {
                var checkForFourteen = currentUserVacations.Where(x => x.Days == 14 && x.StartDate.Year == DateTime.Now.Year).ToList();
                var checkForSeven = currentUserVacations.Where(x => x.Days == 7 && x.StartDate.Year == DateTime.Now.Year).ToList();

                if (checkForFourteen.Any() && days == 14)
                {
                    return BadRequest("ERROR: Teil on juba sel aastal olnud/paigas puhkus pikkusega 14 päeva");
                }

                if (checkForSeven.Any() && days == 7)
                {
                    return BadRequest("ERROR: Teil on juba sel aastal olnud/paigas puhkus pikkusega 7 päeva");
                }

                employee.VacationDays -= days;
                EmployeeRepository.SaveOrUpdate(employee);
                EmployeeRepository.Commit();

                var vacation = new Vacation()
                {
                    StartDate = model.Start,
                    EndDate = model.End,
                    Status = VacationStatus.Pending,
                    EmployeeId = id,
                    Days = days,
                    Comments = model.Comment
                };

                VacationRepository.AddIfNew(vacation);
                VacationRepository.Commit();

                var emails = EmployeeRepository.Where(x => x.VacationMessages == true && x.Role.Rights.HasFlag(AccessRights.Vacations)).Select(x => x.Email).ToList();
                EmailHelper.VacationRequested(vacation, emails);

                return Ok();
            }
            return BadRequest("ERROR: Valitud puhkuse pikkus ei vasta eeskirjadele, lubatud on üks 14 päevane puhkus, üks 7 päevane puhkus ja ülejäänud puhkused on lühemad kui 7 päeva");
        }

        
        [HttpGet]
        [AuthorizeApi(AccessRights.Standard)]
        public IHttpActionResult GetMyVacation()
        {
            return Ok(VacationRepository
                .Where(x => x.EmployeeId == CurrentUser.Id)
                .Where(x => x.Status == VacationStatus.Approved || x.Status == VacationStatus.Pending)
                .AsDto());
        }

        
        [HttpPost]
        [AuthorizeApi(AccessRights.Standard)]
        public IHttpActionResult CancelVacation(GeneralIdModel model)
        {
            var vacation = VacationRepository.First(x => x.Id == model.Id);
            var employee = EmployeeRepository.First(x => x.Id == CurrentUser.Id);

            employee.VacationDays += vacation.Days;
            vacation.Status = VacationStatus.Declined;

            EmployeeRepository.SaveOrUpdate(employee);
            VacationRepository.SaveOrUpdate(vacation);

            EmployeeRepository.Commit();
            VacationRepository.Commit();
            return Ok();
        }

        
        [HttpGet]
        [AuthorizeApi(AccessRights.Standard)]
        public IHttpActionResult GetVacationDays()
        {
            return Ok(EmployeeRepository.FirstOrDefault(x => x.Id == CurrentUser.Id)?.VacationDays);
        }
    }
}