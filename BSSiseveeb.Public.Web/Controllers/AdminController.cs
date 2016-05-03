using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Data;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BSSiseveeb.Public.Web.Controllers
{
    [AuthorizeLevel(AccessRights.Level4)]
    public class AdminController : BaseController
    {
        private PasswordHasher _hasher;

        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Level5)]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Level5)]
        public ActionResult EditEmployees()
        {
            return View(new WorkersViewModel() {Employees = EmployeeRepository.AsDto().ToList()});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeApi(AccessRights.Level5)]
        public ActionResult SetEmployee(RegistrationModel model)
        {
            _hasher = new PasswordHasher();
            var roleId = RoleManager.Roles.Single(x => x.Name == "User").Id;
            var latestId = EmployeeRepository.AsDto().Max(x => x.Id);
            var employee = new Employee
            {
                Name = model.Name,
                Birthdate = model.BirthDay,
                ContractEnd = model.End,
                ContractStart = model.Start,
                PhoneNumber = model.Phone,
                VacationDays = model.VacationDays,
                Email = model.Email,
                Id = latestId + 1
            };

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                EmployeeId = employee.Id,
                AccessFailedCount = 0,
                PasswordHash = _hasher.HashPassword(model.Password),
                RoleId = roleId,
                Messages = "no",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                Employee = employee
            };

            UserManager.Create(user);
            HttpContext.GetOwinContext().Get<BSContext>().SaveChanges();
            UserManager.AddToRole(user.Id, "user");

            return View("Index");
        }

        [AuthorizeApi(AccessRights.Level5)]
        public ActionResult ViewEmployee(int id)
        {
            var employee = EmployeeRepository.AsDto().First(x => x.Id == id);
            var user = UserManager.FindByEmail(employee.Email);
            var role = RoleManager.FindById(user.RoleId);
            var roles = RoleManager.Roles.Select(x => x.Name).ToList();

            var model = new RegistrationModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Start = employee.ContractStart,
                End = employee.ContractEnd,
                Phone = employee.PhoneNumber,
                VacationDays = employee.VacationDays,
                Username = user.UserName,
                OldRole = role.Name,
                NewRole = role.Name,
                Roles = roles
            };

            return View("EditEmployee", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeApi(AccessRights.Level5)]
        public ActionResult EditEmployee(RegistrationModel model)
        {
            var employee = EmployeeRepository.First(x => x.Id == model.Id);
            var roleId = RoleManager.Roles.Single(x => x.Name == model.NewRole).Id;

            employee.VacationDays = model.VacationDays;
            employee.ContractEnd = model.End;
            employee.ContractStart = model.Start;
            employee.Email = model.Email;
            employee.PhoneNumber = model.Phone;
            employee.Name = model.Name;
            employee.Account.Email = model.Email;
            employee.Account.UserName = model.Username;
            employee.Account.RoleId = roleId;


            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();

            UserManager.Update(employee.Account);
            HttpContext.GetOwinContext().Get<BSContext>().SaveChanges();
            UserManager.RemoveFromRole(employee.Account.Id, model.OldRole);
            UserManager.AddToRole(employee.Account.Id, model.NewRole);

            return View("Index");
        }
    }
}