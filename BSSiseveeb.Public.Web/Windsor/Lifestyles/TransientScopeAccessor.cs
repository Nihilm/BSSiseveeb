using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace BSSiseveeb.Public.Web.Windsor.Lifestyles
{
    public class TransientScopeAccessor : IScopeAccessor
    {
        public ILifetimeScope GetScope(CreationContext context)
        {
            return new DefaultLifetimeScope();
        }

        public void Dispose()
        {
        }
    }
}