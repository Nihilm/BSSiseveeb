﻿using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Controllers.Helpers;
using BSSiseveeb.Public.Web.Models;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BSSiseveeb.Public.Web.Controllers
{
    [Authorize]
    public partial class AdminController : BaseController
    {
        [AuthorizeLevel(AccessRights.Vacations)]
        public virtual ActionResult Vacations()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Requests)]
        public virtual ActionResult Requests()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Users)]
        public virtual ActionResult EditEmployees()
        {
            return View(new WorkersViewModel() { Employees = EmployeeRepository.AsDto() });
        }

        [AuthorizeLevel(AccessRights.Users)]
        public virtual ActionResult ViewEmployee(string id)
        {
            var employee = EmployeeRepository.First(x => x.Id == id);
            var role = RoleRepository.First(x => x.Id == employee.RoleId);
            var roles = RoleRepository.Select(x => x.Name).ToList();

            var model = new RegistrationModel
            {
                Id = employee.Id,
                Start = employee.ContractStart,
                End = employee.ContractEnd,
                VacationDays = employee.VacationDays,
                Phone = employee.PhoneNumber,
                Skype = employee.Skype,
                SocialSecurityID = employee.SocialSecurityID,
                OldRole = role.Name,
                NewRole = role.Name,
                Roles = roles,
            };

            return View(MVC.Admin.Views.EditEmployee, model);
        }

        [HttpPost]
        [AuthorizeLevel(AccessRights.Users)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult EditEmployee(RegistrationModel model)
        {
            var employee = EmployeeRepository.First(x => x.Id == model.Id);
            var role = RoleRepository.First(x => x.Name == model.NewRole);

            employee.VacationDays = model.VacationDays;
            employee.ContractEnd = model.End;
            employee.ContractStart = model.Start;
            employee.PhoneNumber = model.Phone;
            employee.Skype = model.Skype;
            employee.SocialSecurityID = model.SocialSecurityID;

            if (model.BirthDay != null)
            {
                employee.Birthdate = model.BirthDay;
            }

            employee.Role = role;

            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();


            return View(MVC.Admin.Views.EditEmployees, new WorkersViewModel() { Employees = EmployeeRepository.AsDto() });
        }

        [HttpPost]
        [AuthorizeLevel(AccessRights.Requests)]
        public virtual ActionResult GeneratePdf(GeneratePdfModel model)
        {
            if (model.Start == new DateTime() || model.End == new DateTime())
            {
                return View(MVC.Admin.Views.Requests, new GeneratePdfModel());
            }

            var status = new RequestStatus();

            switch (model.Status)
            {
                case "Confirmed":
                    status = RequestStatus.Confirmed;
                    break;
                case "Declined":
                    status = RequestStatus.Declined;
                    break;
                case "Cancelled":
                    status = RequestStatus.Cancelled;
                    break;
                default:
                    status = RequestStatus.Pending;
                    break;
            }

            var requests = RequestRepository
                .Where(x => x.TimeStamp < model.End && x.TimeStamp > model.Start && x.Status == status);
            var start = model.Start.ToString("d");
            var end = model.End.ToString("d");

            var stream = PdfHelper.BuildRequestsPdf(requests, start, end, status);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.BinaryWrite(stream.ToArray());
            Response.Flush();
            stream.Close();
            Response.End();

            return View(MVC.Admin.Views.Requests, new GeneratePdfModel());
        }

        [HttpGet]
        [AuthorizeLevel(AccessRights.Users)]
        public virtual async Task<ActionResult> SyncUsers()
        {
            string token = await ADService.GetTokenForApplication();
            if (token == null)
            {
                return RedirectToAction("SignIn", MVC.Account.Name);
            }

            var result = await ADService.GetUsers();
            IEnumerable<IUser> users = result.CurrentPage.ToList();

            var myId = CurrentUserId;
            var defaultRole = RoleRepository.Single(x => x.Name == "User");
            var currentEmployees = EmployeeRepository.Select(x => x.Id);

            var appUsers = users.Where(x => x.GivenName != null).Select(x => new Employee()
            {
                Id = x.ObjectId,
                Email = x.Mail,
                Role = defaultRole,
                Name = x.DisplayName,
                PhoneNumber = x.Mobile,
                IsInitialized = false,
                VacationMessages = false,
                RequestMessages = false,
                MonthlyBirthdayMessages = false,
                DailyBirthdayMessages = false,
                VacationDays = 28
            });

            var newUsers = appUsers.Where(x => !currentEmployees.Contains(x.Id));

            EmployeeRepository.AddRange(newUsers);

            EmployeeRepository.Commit();

            return View(MVC.Admin.Views.EditEmployees, new WorkersViewModel() { Employees = EmployeeRepository.AsDto() });
        }

        [HttpPost]
        [AuthorizeLevel(AccessRights.Users)]
        public virtual ActionResult ParseCsv(CsvImportViewModel model)
        {
            var parsedEmployees = CsvImportService.parseEmployees(model.CsvFile.InputStream);

            var employees = EmployeeRepository.ToList();
            foreach (var parsedEmployee in parsedEmployees)
            {
                try
                {
                    var employee = employees.Single(x => x.Name == parsedEmployee.Name);
                    employee.Birthdate = parsedEmployee.Birthdate;
                    employee.ContractStart = parsedEmployee.ContractStart;
                    employee.SocialSecurityID = parsedEmployee.SocialSecurityID;
                    employee.PhoneNumber = parsedEmployee.PhoneNumber;
                    employee.Skype = parsedEmployee.Skype;
                    employee.Email = parsedEmployee.Email;
                    EmployeeRepository.SaveOrUpdate(employee);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            EmployeeRepository.Commit();

            return (View(MVC.Admin.Views.EditEmployees, new WorkersViewModel() { Employees = EmployeeRepository.AsDto() }));
        }
    }
}