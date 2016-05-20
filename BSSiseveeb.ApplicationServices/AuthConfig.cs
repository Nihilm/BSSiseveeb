using System.Configuration;

namespace BSSiseveeb.ApplicationServices
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
}
