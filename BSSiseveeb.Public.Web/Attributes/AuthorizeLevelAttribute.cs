using System.Web;
using System.Web.Mvc;
using System.Linq;
using BSSiseveeb.Core.Domain;
using System;
using System.Security.Claims;
using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Data.Repositories;

namespace BSSiseveeb.Public.Web.Attributes
{

    public class AuthorizeLevelAttribute : AuthorizeAttribute
    {
         /*public AccessRights Rights { get; set; }

            public AuthorizeLevelAttribute(AccessRights rights, AccessRights check)
            {
                this.Rights = rights;
            }

            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {

                var identity = (ClaimsIdentity)ClaimsPrincipal.Current.Identity;

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

