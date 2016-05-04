using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BSSiseveeb.Public.Web.Models;

namespace BSSiseveeb.Public.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        [AuthorizeLevel(AccessRights.Standard)]
        public ActionResult Index()
        {
            var employee = EmployeeRepository.Single(x => x.Id == CurrentUser.EmployeeId);
            var model = new ChangeAccountSettingsViewModel()
            {
                Phone = employee.PhoneNumber,
                Email = employee.Email,
                Messages = employee.Account.Messages
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAccount(ChangeAccountSettingsViewModel model)
        {

            var employeeId = CurrentUser.EmployeeId;
            var employee = EmployeeRepository.First(x => x.Id == employeeId);
            employee.Email = model.Email;
            employee.PhoneNumber = model.Phone;

            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();

            CurrentUser.Email = model.Email;
            CurrentUser.Messages = model.Messages;
            UserManager.Update(CurrentUser);

            if (model.NewPassword != null)
            {
                var userId = CurrentUser.Id;
                var result = UserManager.ChangePassword(userId, model.OldPassword, model.NewPassword);
                model.OldPassword = "";
                model.NewPassword = "";
                model.ConfirmPassword = "";

                if (result.Succeeded)
                {
                    model.Message = "Parooli vahetus õnnestus";
                    return View("Index", model);
                }

                model.Message = "Parooli vahetus ebaõnnestus";
                return View("Index", model);
            }

            model.OldPassword = "";
            model.NewPassword = "";
            model.ConfirmPassword = "";
            model.Message = "Teie andmed on salvestatud";
            return View("Index", model);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}