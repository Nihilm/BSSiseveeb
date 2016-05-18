using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

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
    }
}