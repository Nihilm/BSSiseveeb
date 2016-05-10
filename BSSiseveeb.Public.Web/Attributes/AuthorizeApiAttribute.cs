using System.Web.Http;
using System.Web.Http.Controllers;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Contracts.Repositories;
using System.Security.Claims;
using BSSiseveeb.Core;
using System.Linq;
using System;
using BSSiseveeb.Data.Repositories;

namespace BSSiseveeb.Public.Web.Attributes
{
    public class AuthorizeApiAttribute : AuthorizeAttribute
    {
      /*  
        public AccessRights Rights { get; set; }
        public AccessRights Check { get; set; }

        public AuthorizeApiAttribute(AccessRights rights, AccessRights check)
        {
            this.Rights = rights;
            this.Check = check;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var repository = new EmployeeRepository();

            var identity = (ClaimsIdentity)ClaimsPrincipal.Current.Identity;
            var id = identity.FindFirst(AppClaims.ObjectIdentifier).Value;
            var employee = repository.FirstOrDefault(x => x.Id == id);

            try
            {
                if (identity.IsAuthenticated &&
                    Rights.HasFlag(Check))
                {
                    return true;
                }

                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }*/
    }
}