using System.Web;
using MvcFlash.Core;
using MvcFlash.Core.Providers;
using StructureMap.Configuration.DSL;

namespace MvcFlash.Sample.DependencyResolution.Registries
{
    public class FlashRegistry : Registry
    {
        public FlashRegistry()
        {
            For<HttpContextBase>()
                .HybridHttpOrThreadLocalScoped()
                .Use(() => new HttpContextWrapper(HttpContext.Current));

            For<IFlashMessenger>()
                .HybridHttpOrThreadLocalScoped()
                .Use<HttpSessionFlashMessenger>();
        }
    }
}