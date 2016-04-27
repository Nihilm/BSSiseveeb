using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    
    public class AdminApiController : BaseApiController
    {
        [AuthorizeApi(AccessRights.Level5)]
        [HttpGet]
        public IEnumerable<Vacation> GetPendingVacations()
        {
            return VacationRepository.Where(x => x.Status == VacationStatus.Pending).OrderBy(x => x.StartDate).ToList();
        }

        [AuthorizeApi(AccessRights.Level5)]
        [HttpGet]
        public IEnumerable<Request> GetPendingRequests()
        {
            return RequestRepository.Where(x => x.Status == RequestStatus.Pending).OrderBy(x => x.TimeStamp).ToList();
        }

        [AuthorizeApi(AccessRights.Level5)]
        [HttpPost]
        public IHttpActionResult ApproveVacation(ApiControllerModels.GeneralIdModel model)
        {
            var id = model.Id;
            var vacation = VacationRepository.First(x => x.Id == id);
            vacation.Status = VacationStatus.Approved;
            VacationRepository.SaveOrUpdate(vacation);
            VacationRepository.Commit();
            return Ok();
        }

        [AuthorizeApi(AccessRights.Level5)]
        [HttpPost]
        public IHttpActionResult ApproveRequest(ApiControllerModels.GeneralIdModel model)
        {
            var id = model.Id;
            var request = RequestRepository.First(x => x.Id == id);
            request.Status = RequestStatus.Confirmed;
            RequestRepository.SaveOrUpdate(request);
            RequestRepository.Commit();
            return Ok();
        }

        [AuthorizeApi(AccessRights.Level5)]
        [HttpPost]
        public IHttpActionResult DeclineRequest(ApiControllerModels.GeneralIdModel model)
        {
            var id = model.Id;
            var request = RequestRepository.First(x => x.Id == id);
            request.Status = RequestStatus.Declined;
            RequestRepository.SaveOrUpdate(request);
            RequestRepository.Commit();
            return Ok();
        }

        [AuthorizeApi(AccessRights.Level5)]
        [HttpGet]
        public IEnumerable<Vacation> GetConfirmedVacations()
        {
            return VacationRepository.Where(x => x.Status == VacationStatus.Approved && x.EndDate > DateTime.Now).ToList();
        }

        [AuthorizeApi(AccessRights.Level5)]
        [HttpPost]
        public IHttpActionResult ModifyVacation(ApiControllerModels.VacationModel model)
        {
            var id = model.Id;
            if (model.End < model.Start)
            {
                return BadRequest();
            }

            var vacation = VacationRepository.First(x => x.Id == id);
            var currentUser = CurrentUser().EmployeeId;
            var employee = EmployeeRepository.First(x => x.Id == currentUser);
            var tempDays = vacation.Days;

            vacation.StartDate = model.Start;
            vacation.EndDate = model.End;
            vacation.Days = (int)model.End.Subtract(model.Start).TotalDays + 1;
            tempDays -= vacation.Days;

            if (employee.VacationDays - tempDays < 0)
            {
                return BadRequest();
            }

            employee.VacationDays -= tempDays;

            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();

            VacationRepository.SaveOrUpdate(vacation);
            VacationRepository.Commit();

            return Ok();
        }

        [AuthorizeApi(AccessRights.Level5)]
        [HttpPost]
        public IHttpActionResult DeleteVacation(ApiControllerModels.GeneralIdModel model)
        {
            var id = model.Id;
            var vacation = VacationRepository.First(x => x.Id == id);
            var currentUser = CurrentUser().EmployeeId;
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