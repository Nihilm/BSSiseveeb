using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Data;
using BSSiseveeb.Public.Web.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BSSiseveeb.Public.Web.Controllers
{
    public abstract partial class BaseController : Controller
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IVacationRepository VacationRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public IBSContextContextManager ContextManager { get; set; }

        public string CurrentUserId
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return null;
                }

                var identity = (ClaimsIdentity)ClaimsPrincipal.Current.Identity;
                return identity.FindFirst(AppClaims.ObjectIdentifier).Value;
            }
        }

        public Employee CurrentUser
        {
            get
            {
                var user = EmployeeRepository.FirstOrDefault(x => x.Id == CurrentUserId);
                if (user == null)
                {
                    user = new Employee()
                    {
                        Role = new Role() { Rights = AccessRights.None }
                    };
                }

                return user;
            }
        }

        public RoleDto CurrentUserRole
        {
            get
            {
                return CurrentUser.Role.AsDto();
            }
        }

        public async Task<string> GetTokenForApplication()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            string userObjectID = ClaimsPrincipal.Current.FindFirst(AppClaims.ObjectIdentifier).Value;

            try
            {
                // get a token for the Graph without triggering any user interaction (from the cache, via multi-resource refresh token, etc)
                ClientCredential clientcred = new ClientCredential(AuthConfig.ClientId, AuthConfig.AppKey);
                // initialize AuthenticationContext with the token cache of the currently signed in user, as kept in the app's database
                AuthenticationContext authenticationContext = new AuthenticationContext(AuthConfig.AadInstance + AuthConfig.TenantId, new ADALTokenCache(signedInUserID, ContextManager));
                AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenSilentAsync(AuthConfig.GraphResourceId, clientcred, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                return authenticationResult.AccessToken;
            }
            catch (AdalSilentTokenAcquisitionException ex)
            {
                return null;
            }
        }
    }
}