using BSSiseveeb.Data;
using BSSiseveeb.Data.Repositories;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sparkling.Data.Repositories;
using Sparkling.DataInterfaces;

namespace BSSiseveeb.Public.Web.Windsor.Installers
{
    public class DalInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
               // Contextmanager
               Component.For<IContextManager, IBSContextContextManager>()
                   .ImplementedBy<BSContextDbContextManager>()
                .LifestylePerWebRequest(),

               Component.For(typeof(IRepository<>))
                   .ImplementedBy(typeof(Repository<>))
                .LifestylePerWebRequest(),


               Component.For(typeof(IRepositoryWithTypedId<,>))
                   .ImplementedBy(typeof(RepositoryWithTypedId<,>))
                .LifestylePerWebRequest(),


               Component.For(typeof(IRepositoryWithNoId<>))
                   .ImplementedBy(typeof(RepositoryWithNoId<>))
               .LifestylePerWebRequest(),

               // Repositories
               Classes.FromAssemblyContaining(typeof(RequestRepository))
                   .BasedOn<IRepository>()
                   .WithService.FromInterface()
                   .LifestylePerWebRequest()
               );
        }
    }
}