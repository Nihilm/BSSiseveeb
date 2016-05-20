using BSSiseveeb.ApplicationServices.Services;
using BSSiseveeb.Core.Contracts.Services;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BSSiseveeb.Public.Web.Windsor.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
               // Repositories
               Classes.FromAssemblyContaining(typeof(ADService))
                   .BasedOn<IApplicationService>()
                   .WithService.FromInterface()
                   .LifestyleTransient()
               );
        }
    }
}