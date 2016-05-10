using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Data;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Controllers.API.Helpers;
using BSSiseveeb.Public.Web.Models;


namespace BSSiseveeb.Public.Web.Controllers.API
{
    [Authorize]
    public class AdminApiController : BaseApiController
    {
        public IBSContextContextManager ContextManager { get; set; }

        [HttpGet]
        public IHttpActionResult GetPendingVacations()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Vacations))
            {
                return BadRequest("ERROR: Teil Puuduvad õigused puhkuste üle");
            }

            return Ok(VacationRepository
            .Where(x => x.Status == VacationStatus.Pending)
            .OrderBy(x => x.StartDate)
            .AsDto());
        }

        [HttpGet]
        public IHttpActionResult GetPendingRequests()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Requests))
            {
                return BadRequest("ERROR: Teil Puuduvad õigused taotluste üle");
            }
            return Ok(RequestRepository
                .Where(x => x.Status == RequestStatus.Pending)
                .OrderBy(x => x.TimeStamp)
                .AsDto());
        }

        [HttpPost]
        public IHttpActionResult ApproveVacation(GeneralIdModel model)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Vacations))
            {
                return BadRequest("ERROR: Teil Puuduvad õigused puhkuste üle");
            }

            var vacation = VacationRepository.FirstOrDefault(x => x.Id == model.Id);
            if (vacation == null)
            {
                return BadRequest("ERROR: Puhkus puudub andmebaasis");
            }

            vacation.Status = VacationStatus.Approved;
            VacationRepository.SaveOrUpdate(vacation);
            VacationRepository.Commit();

            var emails = EmployeeRepository
                .Where(x => x.VacationMessages == true && x.Id == vacation.EmployeeId)
                .Select(x => x.Email)
                .ToList();

            var subject = "Approved Vacation";
            var body = "<p>Your Vacation has been Approved</p>";
            EmailHelper.SendEmail(emails, subject, body);

            return Ok();
        }

        
        [HttpPost]
        public IHttpActionResult ApproveRequest(GeneralIdModel model)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Requests))
            {
                return BadRequest("ERROR: Teil Puuduvad õigused taotluste üle");
            }

            var request = RequestRepository.FirstOrDefault(x => x.Id == model.Id);
            if (request == null)
            {
                return BadRequest("ERROR: Request puudub andmebaasis");
            }

            request.Status = RequestStatus.Confirmed;
            RequestRepository.SaveOrUpdate(request);
            RequestRepository.Commit();

            var emails = EmployeeRepository.Where(x => x.RequestMessages == true && x.Id == request.EmployeeId).Select(x => x.Email).ToList();
            var subject = "Approved Request";
            var body = "<p>Your Request has been approved</p>";
            EmailHelper.SendEmail(emails, subject, body);

            return Ok();
        }

        
        [HttpPost]
        public IHttpActionResult DeclineRequest(GeneralIdModel model)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Requests))
            {
                return BadRequest("ERROR: Teil Puuduvad õigused taotluste üle");
            }

            var request = RequestRepository.First(x => x.Id == model.Id);
            request.Status = RequestStatus.Declined;
            RequestRepository.SaveOrUpdate(request);
            RequestRepository.Commit();

            var emails = EmployeeRepository.Where(x => x.RequestMessages == true && x.Id == request.EmployeeId).Select(x => x.Email).ToList();
            var subject = "Denied Request";
            var body = "<p>Your Request has been denied</p>";
            EmailHelper.SendEmail(emails, subject, body);

            return Ok();
        }

        
        [HttpGet]
        public IHttpActionResult GetConfirmedVacations()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Vacations))
            {
                return BadRequest("ERROR: Teil Puuduvad õigused puhkuste üle");
            }
            return Ok(VacationRepository
                .Where(x => x.Status == VacationStatus.Approved && x.EndDate > DateTime.Now)
                .AsDto());
        }

        
        [HttpPost]
        public IHttpActionResult ModifyVacation(VacationModel model)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Vacations))
            {
                return BadRequest("ERROR: Teil Puuduvad õigused puhkuste üle");
            }

            if (model.End < model.Start)
            {
                return BadRequest();
            }

            var vacation = VacationRepository.First(x => x.Id == model.Id);
            var employee = EmployeeRepository.First(x => x.Id == vacation.Employee.Id);
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

            var emails = EmployeeRepository.Where(x => x.VacationMessages == true && x.Id == vacation.EmployeeId).Select(x => x.Email).ToList();
            var subject = "Vacation modified";
            var body = "<p>Your vacation has been modified</p>";
            EmailHelper.SendEmail(emails, subject, body);

            return Ok();
        }

        
        [HttpPost]
        public IHttpActionResult DeleteVacation(GeneralIdModel model)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Vacations))
            {
                return BadRequest("ERROR: Teil Puuduvad õigused puhkuste üle");
            }

            var vacation = VacationRepository.First(x => x.Id == model.Id);
            var employee = EmployeeRepository.First(x => x.Id == vacation.Employee.Id);

            employee.VacationDays += vacation.Days;
            vacation.Status = VacationStatus.Declined;

            EmployeeRepository.SaveOrUpdate(employee);
            VacationRepository.SaveOrUpdate(vacation);

            var emails = EmployeeRepository.Where(x => x.VacationMessages == true && x.Id == vacation.EmployeeId).Select(x => x.Email).ToList();
            var subject = "Vacation denied";
            var body = "<p>Your vacation has been denied</p>";
            EmailHelper.SendEmail(emails, subject, body);

            ContextManager.Commit();
            return Ok();
        }
    }
}