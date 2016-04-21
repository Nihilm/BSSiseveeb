using System.Web;
using System.Web.Mvc;
using BSSiseveeb.Core.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace BSSiseveeb.Public.Web.Attributes
{
    public class AuthorizeLevel2Attribute : AuthorizeAttribute
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
                    .Rights.HasFlag(AccessRights.Level2))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
