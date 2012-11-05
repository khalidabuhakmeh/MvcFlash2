using System.Web.Mvc;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;

namespace MvcFlash.Sample.Controllers
{
    public class IoCController : Controller
    {
        private readonly IFlashMessenger _flash;

        public IoCController(IFlashMessenger flash)
        {
            _flash = flash;
        }

        public ActionResult Index()
        {
            _flash.Success("Well Done!", "You successfully read this important alert message.");
            _flash.Warning("Warning!", "You should really read this message");
            _flash.Info("Info!", "you can read this... if you want.");
            _flash.Error("Error!", "The sky is falling!");

            return View();
        }

    }
}
