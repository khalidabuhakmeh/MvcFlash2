using System;
using System.Web.Mvc;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using MvcFlash.Core.Providers;
using StructureMap;

namespace MvcFlash.Sample.Controllers
{
    public class RavenDbController : Controller
    {
        protected Func<FlashSettings> Default { get; set; }
        
        public ActionResult Index()
        {
            Flash.Instance.Success("RavenDB is the bomb!");
            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ObjectFactory.Container.Configure(x => x.For<IFlashMessenger>()
                                                    .Use(() => new RavenDbFlashMessenger()));
            Flash.Reset();
            base.OnActionExecuting(filterContext);
        }


        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            ObjectFactory.Container.Configure(x => x.For<IFlashMessenger>()
                                                    .Use(() => new HttpSessionFlashMessenger()));
            Flash.Reset();
        }
    }
}
