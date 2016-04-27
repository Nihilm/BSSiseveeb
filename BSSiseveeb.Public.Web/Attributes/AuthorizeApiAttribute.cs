using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using BSSiseveeb.Core.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace BSSiseveeb.Public.Web.Attributes
{
    public class AuthorizeApiAttribute : AuthorizeAttribute
    {
        public AccessRights Rights { get; set; }

        public AuthorizeApiAttribute(AccessRights rights)
        {
            this.Rights = rights;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var httpContext = HttpContext.Current;
            var owin = httpContext.GetOwinContext();
            var roleManager = owin.GetUserManager<ApplicationRoleManager>();
            var userManager = owin.GetUserManager<ApplicationUserManager>();
            var id = httpContext.User.Identity.GetUserId();

            try
            {
                if (httpContext.User.Identity.IsAuthenticated &&
                    roleManager.FindById(userManager.FindById(id).RoleId)
                    .Rights.HasFlag(Rights))
                {
                    return true;
                }

                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }
}