using BSSiseveeb.ApplicationServices.Models;
using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Services;
using BSSiseveeb.Data;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security.Notifications;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace BSSiseveeb.ApplicationServices.Services
{
    public class ADService : IADService
    {
        public IBSContextContextManager ContextManager { get; set; }
        

        public async Task<AuthenticationResult> ADStartUp(AuthorizationCodeReceivedNotification context, String authority)
        {
            var dbContext = IoC.Resolve<IBSContextContextManager>();

            var code = context.Code;
            ClientCredential credential = new ClientCredential(AuthConfig.ClientId, AuthConfig.AppKey);
            string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            AuthenticationContext authContext = new AuthenticationContext(authority, new ADALTokenCache(signedInUserID, dbContext));
            var result = authContext.AcquireTokenByAuthorizationCodeAsync(
                code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, AuthConfig.GraphResourceId);

            return await result;
        }

        public async Task<IPagedCollection<IUser>> GetUsers()
        {
            Uri servicePointUri = new Uri(AuthConfig.GraphResourceId);
            Uri serviceRoot = new Uri(servicePointUri, AuthConfig.TenantId);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                  async () => await GetTokenForApplication());

            return await activeDirectoryClient.Users.ExecuteAsync();
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