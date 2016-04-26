using System;
using System.Web;
using System.Web.Mvc;
using BSSiseveeb.Core.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace BSSiseveeb.Public.Web.Attributes
{

    public class AuthorizeLevelAttribute : AuthorizeAttribute
    {
        public AccessRights Rights { get; set; }

        public AuthorizeLevelAttribute(AccessRights rights)
        {
            this.Rights = rights;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
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

