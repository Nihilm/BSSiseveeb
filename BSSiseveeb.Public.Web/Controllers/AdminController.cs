using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Data;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BSSiseveeb.Public.Web.Controllers
{
    public class AdminController : BaseController
    {
        private PasswordHasher _hasher;

        [AuthorizeLevel(AccessRights.Level4)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Level5)]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [AuthorizeApi(AccessRights.Level5)]
        public ActionResult SetEmployee(RegistrationModel model)
        {
            _hasher = new PasswordHasher();
            var roleId = RoleManager.Roles.Single(x => x.Name == "User").Id;
            var employee = new Employee
            {
                Name = model.Name,
                Birthdate = model.BirthDay,
                ContractEnd = model.End,
                ContractStart = model.Start,
                PhoneNumber = model.Phone,
                VacationDays = model.VacationDays,
                Email = model.Email
            };

            EmployeeRepository.Add(employee);
            EmployeeRepository.Commit();
            employee = EmployeeRepository.Single(x => x.Name == model.Name);

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
                SecurityStamp = Guid.NewGuid().ToString()
            };

            UserManager.Create(user);
            HttpContext.GetOwinContext().Get<BSContext>().SaveChanges();
            UserManager.AddToRole(user.Id, "user");

            return View("Index");
        }
    }
}