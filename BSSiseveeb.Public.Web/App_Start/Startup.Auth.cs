using BSSiseveeb.ApplicationServices;
using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Services;
using BSSiseveeb.Data;
using BSSiseveeb.Public.Web.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.IdentityModel.Claims;
using System.Web;
using System.Web.Helpers;

namespace BSSiseveeb.Public.Web
{
    public partial class Startup
    {
        public static readonly string Authority = AuthConfig.AadInstance + AuthConfig.TenantId;

        public void ConfigureAuth(IAppBuilder app)
        {
            var dbContext = IoC.Resolve<IBSContextContextManager>();
            var ADService = IoC.Resolve<IADService>();

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
                            return ADService.ADStartUp(context, Authority);
                        }
                    }
                });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}