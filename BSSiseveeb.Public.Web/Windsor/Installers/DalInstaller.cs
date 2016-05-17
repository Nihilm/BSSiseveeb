using BSSiseveeb.Data;
using BSSiseveeb.Data.Repositories;
using BSSiseveeb.Public.Web.Windsor.Lifestyles;
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
                .LifestyleScoped<HybridScopeAccessor>(),

               Component.For(typeof(IRepository<>))
                   .ImplementedBy(typeof(Repository<>))
                .LifestyleScoped<HybridScopeAccessor>(),


               Component.For(typeof(IRepositoryWithTypedId<,>))
                   .ImplementedBy(typeof(RepositoryWithTypedId<,>))
                .LifestyleScoped<HybridScopeAccessor>(),


               Component.For(typeof(IRepositoryWithNoId<>))
                   .ImplementedBy(typeof(RepositoryWithNoId<>))
                .LifestyleScoped<HybridScopeAccessor>(),

               // Repositories
               Classes.FromAssemblyContaining(typeof(RequestRepository))
                   .BasedOn<IRepository>()
                   .WithService.FromInterface()
                   .LifestyleScoped<HybridScopeAccessor>()
               );
        }
    }
}