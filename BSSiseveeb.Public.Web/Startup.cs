using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(BSSiseveeb.Public.Web.Startup))]
namespace BSSiseveeb.Public.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
