using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BSSiseveeb.Core.Domain;


namespace BSSiseveeb.Public.Web.Attributes
{

    public class AuthorizeLevel5Attribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    if (httpContext.GetOwinContext()
                        .GetUserManager<ApplicationRoleManager>()
                        .FindById(httpContext.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>()
                            .FindById(httpContext.User.Identity.GetUserId()).RoleId)
                        .Rights.HasFlag(AccessRights.Level5))
                    {
                        return true;
                    }
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
