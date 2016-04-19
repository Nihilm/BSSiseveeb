using System.Web.Http.Controllers;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BSSiseveeb.Public.Web.Windsor.Installers
{
    public class ControllerInstaller : IWindsorInstaller
    {
        private const string Controller = "Controller";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IController>()
                    .If(t => t.Name.EndsWith(Controller))
                    .LifestyleTransient(),

                Classes.FromThisAssembly()
                    .BasedOn<IHttpController>()
                    .If(t => t.Name.EndsWith(Controller))
                    .LifestyleTransient()
                );
        }
    }
}