using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BSSiseveeb.Core.Domain;
using System.Web.Mvc;


namespace BSSiseveeb.Public.Web.Attributes
{

    public class AuthorizeLevel3Attribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                if (httpContext.GetOwinContext()
                    .GetUserManager<ApplicationRoleManager>()
                    .FindById(httpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(httpContext.User.Identity.GetUserId()).RoleId)
                    .Rights.HasFlag(AccessRights.Level3))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
