using BSSiseveeb.Core;
using BSSiseveeb.Data;
using BSSiseveeb.Public.Web.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;
using System.IdentityModel.Claims;
using System.Web;
using System.Web.Helpers;

namespace BSSiseveeb.Public.Web
{
    public static class AuthConfig
    {
        // This is the resource ID of the AAD Graph API.  We'll need this to request a token to call the Graph API.
        public const string GraphResourceId = "https://graph.windows.net";

        public static readonly string ClientId;
        public static readonly string AppKey;
        public static readonly string AadInstance;
        public static readonly string TenantId;
        public static readonly string PostLogOutRedirectUri;
        public static readonly string RedirectUri;

        static AuthConfig()
        {
            ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
            AppKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
            AadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
            TenantId = ConfigurationManager.AppSettings["ida:TenantId"];
            PostLogOutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
            RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        }
    }

    public partial class Startup
    {
        public static readonly string Authority = AuthConfig.AadInstance + AuthConfig.TenantId;

        public void ConfigureAuth(IAppBuilder app)
        {
            var dbContext = IoC.Resolve<IBSContextContextManager>();

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = AuthConfig.ClientId,
                    Authority = Authority,
                    PostLogoutRedirectUri = AuthConfig.PostLogOutRedirectUri,
                    RedirectUri = AuthConfig.RedirectUri,
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                        AuthorizationCodeReceived = (context) =>
                        {
                            var code = context.Code;
                            ClientCredential credential = new ClientCredential(AuthConfig.ClientId, AuthConfig.AppKey);
                            string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                            AuthenticationContext authContext = new AuthenticationContext(Authority, new ADALTokenCache(signedInUserID, dbContext));
                            var result = authContext.AcquireTokenByAuthorizationCodeAsync(
                                code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, AuthConfig.GraphResourceId);

                            return result;
                        }
                    }
                });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}