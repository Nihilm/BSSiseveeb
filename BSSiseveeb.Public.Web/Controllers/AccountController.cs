using System.Web;
using System.Web.Mvc;
using System.Linq;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;


namespace BSSiseveeb.Public.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        public ActionResult Index()
        {
            var employee = EmployeeRepository.Single(x => x.Id == CurrentUser.Id);
            var model = new ChangeAccountSettingsViewModel()
            {
                Phone = employee.PhoneNumber,
                DailyBirthdayMessages = employee.DailyBirthdayMessages,
                MonthlyBirthdayMessages = employee.MonthlyBirthdayMessages,
                RequestMessages = employee.RequestMessages,
                VacationMessages = employee.VacationMessages,
                CurrentUserRole = CurrentUserRole
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAccount(ChangeAccountSettingsViewModel model)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Standard))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }

            var employee = EmployeeRepository.First(x => x.Id == CurrentUser.Id);
            employee.PhoneNumber = model.Phone;
            employee.MonthlyBirthdayMessages = model.MonthlyBirthdayMessages;
            employee.DailyBirthdayMessages = model.DailyBirthdayMessages;
            employee.RequestMessages = model.RequestMessages;
            employee.VacationMessages = model.VacationMessages;


            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();


            model.Message = "Teie andmed on salvestatud";
            model.CurrentUserRole = CurrentUserRole;
            return View("Index", model);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [AllowAnonymous]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public void SignIn()
        {
            //Send an OpenID Connect sign -in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {
                //Redirect to home page if the user is authenticated.
                return RedirectToAction("Index", "Home");
            }

            return View(new BaseViewModel() { CurrentUserRole = CurrentUserRole });
        }
    }
}