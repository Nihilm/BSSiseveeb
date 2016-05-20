using BSSiseveeb.ApplicationServices;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BSSiseveeb.Public.Web.Controllers
{
    [Authorize]
    public partial class AccountController : BaseController
    {
        [AuthorizeLevel(AccessRights.Standard)]
        public virtual ActionResult Index()
        {
            var employee = EmployeeRepository.Single(x => x.Id == CurrentUser.Id);
            var model = new ChangeAccountSettingsViewModel()
            {
                Phone = employee.PhoneNumber,
                DailyBirthdayMessages = employee.DailyBirthdayMessages,
                MonthlyBirthdayMessages = employee.MonthlyBirthdayMessages,
                RequestMessages = employee.RequestMessages,
                VacationMessages = employee.VacationMessages,
                Skype = employee.Skype
            };

            return View(model);
        }

        [HttpPost]
        [AuthorizeLevel(AccessRights.Standard)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult UpdateAccount(ChangeAccountSettingsViewModel model)
        {
            var employee = EmployeeRepository.First(x => x.Id == CurrentUser.Id);
            employee.PhoneNumber = model.Phone;
            employee.MonthlyBirthdayMessages = model.MonthlyBirthdayMessages;
            employee.DailyBirthdayMessages = model.DailyBirthdayMessages;
            employee.RequestMessages = model.RequestMessages;
            employee.VacationMessages = model.VacationMessages;
            employee.Skype = model.Skype;

            if (model.BirthDay != null)
            {
                employee.Birthdate = model.BirthDay;
            }

            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();


            model.Message = "Teie andmed on salvestatud";
            return View(MVC.Account.Views.Index, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> InitializeAccount(ChangeAccountSettingsViewModel model)
        {
            if (EmployeeRepository.SingleOrDefault(x => x.Id == CurrentUserId) != null)
            {
                var employee = EmployeeRepository.First(x => x.Id == CurrentUser.Id);
                employee.PhoneNumber = model.Phone;
                employee.MonthlyBirthdayMessages = model.MonthlyBirthdayMessages;
                employee.DailyBirthdayMessages = model.DailyBirthdayMessages;
                employee.RequestMessages = model.RequestMessages;
                employee.VacationMessages = model.VacationMessages;
                employee.Skype = model.Skype;
                employee.SocialSecurityID = model.SocialSecurityID;
                employee.IsInitialized = true;

                if (model.BirthDay != null)
                {
                    employee.Birthdate = model.BirthDay;
                }

                EmployeeRepository.SaveOrUpdate(employee);
                EmployeeRepository.Commit();
            }
            else
            {
                IPagedCollection<IUser> result = await ADService.GetUsers();
                IUser user = result.CurrentPage.Single(x => x.ObjectId == CurrentUserId);
                var defaultRole = RoleRepository.Single(x => x.Name == "User");

                var employee = new Employee()
                {
                    Id = user.ObjectId,
                    Name = user.DisplayName,
                    Email = user.Mail,
                    Role = defaultRole,
                    PhoneNumber = model.Phone,
                    IsInitialized = true,
                    VacationDays = 28,
                    VacationMessages = model.VacationMessages,
                    RequestMessages = model.RequestMessages,
                    MonthlyBirthdayMessages = model.MonthlyBirthdayMessages,
                    DailyBirthdayMessages = model.DailyBirthdayMessages,
                    Birthdate = model.BirthDay,
                    Skype = model.Skype,
                    SocialSecurityID = model.SocialSecurityID
                };

                EmployeeRepository.Add(employee);
                EmployeeRepository.Commit();
            }

            var employees = EmployeeRepository
                                    .Where(x => x.Birthdate.HasValue)
                                    .Where(x => x.Birthdate.Value.Month == DateTime.Now.Month && x.Birthdate.Value.Day == DateTime.Now.Day)
                                    .AsDto();

            var vacations = VacationRepository
                                    .Where(x => x.StartDate.Month == DateTime.Now.Month || x.EndDate.Month == DateTime.Now.Month)
                                    .Where(x => x.Status == VacationStatus.Approved)
                                    .OrderBy(x => x.StartDate)
                                    .Select(x => new EmployeeVacationModel()
                                    {
                                        EmployeeName = x.Employee.Name,
                                        StartDate = x.StartDate,
                                        EndDate = x.EndDate
                                    })
                                    .ToList();

            return View(MVC.Home.Views.Index, new IndexViewModel() { Employees = employees, Vacations = vacations });
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [AllowAnonymous]
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = Url.Content("~/") },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action(MVC.Account.SignOutCallback());

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public virtual ActionResult SignOutCallback()
        {
            return RedirectToAction(MVC.Home.Index());
        }
    }
}