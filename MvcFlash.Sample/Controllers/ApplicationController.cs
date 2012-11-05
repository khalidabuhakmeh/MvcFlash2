using System.Web.Mvc;
using MvcFlash.Core;

namespace MvcFlash.Sample.Controllers
{
    public abstract class ApplicationController : Controller
    {
        protected virtual IFlashPusher Flash { get; private set; }

        protected ApplicationController()
        {
            Flash = MvcFlash.Core.Flash.Instance;
        }
    }
}
