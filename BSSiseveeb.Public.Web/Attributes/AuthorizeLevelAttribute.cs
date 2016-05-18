using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            var repo = IoC.Resolve<IEmployeeRepository>();
            var identity = (ClaimsIdentity)ClaimsPrincipal.Current.Identity;
            var id = identity.FindFirst(AppClaims.ObjectIdentifier).Value;
            var employee = repo.FirstOrDefault(x => x.Id == id);

            try
            {
                if (employee == null)
                {
                    return false;
                }
                if (identity.IsAuthenticated &&
                    employee.Role.Rights.HasFlag(Rights))
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

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                //filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                //throw new HttpException(403, "Forbidden");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "UnAuthorized" }));
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}

