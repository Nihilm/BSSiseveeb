using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;
using System.Web;

namespace BSSiseveeb.Public.Web.Windsor.Lifestyles
{
    public class HybridScopeAccessor : IScopeAccessor
    {
        private readonly IScopeAccessor webRequestScopeAccessor = new WebRequestScopeAccessor();
        private readonly IScopeAccessor TransientScopeAccessor = new TransientScopeAccessor();

        public ILifetimeScope GetScope(CreationContext context)
        {
            //if (HttpContext.Current != null && !HttpContext.Current.IsWebSocketRequest && PerWebRequestLifestyleModuleUtils.IsInitialized)
            if (HttpContext.Current != null && PerWebRequestLifestyleModuleUtils.IsInitialized)
                //if (HttpContext.Current != null)
                return webRequestScopeAccessor.GetScope(context);

            return TransientScopeAccessor.GetScope(context);
        }

        public void Dispose()
        {
            webRequestScopeAccessor.Dispose();
            TransientScopeAccessor.Dispose();
        }
    }
}