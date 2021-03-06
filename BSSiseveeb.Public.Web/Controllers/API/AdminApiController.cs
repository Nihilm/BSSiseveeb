﻿using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Data;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace BSSiseveeb.Public.Web.Controllers.API
{
    
    public class AdminApiController : BaseApiController
    {
        public IBSContextContextManager ContextManager { get; set; }

        [HttpGet]
        [AuthorizeApi(AccessRights.Vacations)]
        public IHttpActionResult GetPendingVacations()
        {
            return Ok(VacationRepository
            .Where(x => x.Status == VacationStatus.Pending)
            .OrderBy(x => x.StartDate)
            .AsDto());
        }

        [HttpGet]
        [AuthorizeApi(AccessRights.Requests)]
        public IHttpActionResult GetPendingRequests()
        {
            return Ok(RequestRepository
                .Where(x => x.Status == RequestStatus.Pending)
                .OrderBy(x => x.TimeStamp)
                .AsDto());
        }

        [HttpPost]
        [AuthorizeApi(AccessRights.Vacations)]
        public IHttpActionResult ApproveVacation(GeneralIdModel model)
        {
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

            EmailService.VacationApproved(vacation, emails);

            return Ok();
        }
    
        
        [HttpPost]
        [AuthorizeApi(AccessRights.Requests)]
        public IHttpActionResult ApproveRequest(GeneralIdModel model)
        {
            var request = RequestRepository.FirstOrDefault(x => x.Id == model.Id);
            if (request == null)
            {
                return BadRequest("ERROR: Request puudub andmebaasis");
            }

            request.Status = RequestStatus.Confirmed;
            RequestRepository.SaveOrUpdate(request);
            RequestRepository.Commit();

            var emails = EmployeeRepository.Where(x => x.RequestMessages == true && x.Id == request.EmployeeId).Select(x => x.Email).ToList();
            EmailService.RequestApproved(request, emails);

            return Ok();
        }

        
        [HttpPost]
        [AuthorizeApi(AccessRights.Requests)]
        public IHttpActionResult DeclineRequest(GeneralIdModel model)
        {
            var request = RequestRepository.First(x => x.Id == model.Id);
            request.Status = RequestStatus.Declined;
            RequestRepository.SaveOrUpdate(request);
            RequestRepository.Commit();

            var emails = EmployeeRepository.Where(x => x.RequestMessages == true && x.Id == request.EmployeeId).Select(x => x.Email).ToList();
            EmailService.RequestDenied(request, emails);

            return Ok();
        }

        
        [HttpGet]
        [AuthorizeApi(AccessRights.Vacations)]
        public IHttpActionResult GetConfirmedVacations()
        {
            return Ok(VacationRepository
                .Where(x => x.Status == VacationStatus.Approved && x.EndDate > DateTime.Now)
                .AsDto());
        }

        
        [HttpPost]
        [AuthorizeApi(AccessRights.Vacations)]
        public IHttpActionResult ModifyVacation(VacationModel model)
        {
            if (model.End < model.Start)
            {
                return BadRequest();
            }

            var vacation = VacationRepository.First(x => x.Id == model.Id);
            var employee = vacation.Employee;
            var tempDays = vacation.Days;

            vacation.StartDate = model.Start;
            vacation.EndDate = model.End;
            vacation.Days = (int)model.End.Subtract(model.Start).TotalDays + 1;
            tempDays -= vacation.Days;

            if (employee.VacationDays - tempDays < 0)
            {
                return BadRequest();
            }

            employee.VacationDays += tempDays;

            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();

            VacationRepository.SaveOrUpdate(vacation);
            VacationRepository.Commit();


            var emails = EmployeeRepository.Where(x => x.VacationMessages == true && x.Id == vacation.EmployeeId).Select(x => x.Email).ToList();
            EmailService.VacationModified(vacation, emails);

            return Ok();
        }

        
        [HttpPost]
        [AuthorizeApi(AccessRights.Vacations)]
        public IHttpActionResult DeleteVacation(GeneralIdModel model)
        {
            var vacation = VacationRepository.First(x => x.Id == model.Id);
            var employee = EmployeeRepository.First(x => x.Id == vacation.Employee.Id);

            employee.VacationDays += vacation.Days;
            vacation.Status = VacationStatus.Declined;

            EmployeeRepository.SaveOrUpdate(employee);
            VacationRepository.SaveOrUpdate(vacation);

            var emails = EmployeeRepository.Where(x => x.VacationMessages == true && x.Id == vacation.EmployeeId).Select(x => x.Email).ToList();
            EmailService.VacationDenied(vacation, emails);

            ContextManager.Commit();
            return Ok();
        }
    }
}