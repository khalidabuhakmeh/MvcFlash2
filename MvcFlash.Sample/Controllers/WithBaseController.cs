using System.Web.Mvc;
using MvcFlash.Core.Extensions;

namespace MvcFlash.Sample.Controllers
{
    public class WithBaseController : ApplicationController
    {
        public ActionResult Index()
        {
            Flash.Success("Well Done!", "You successfully read this important alert message.");
            Flash.Warning("Warning!", "You should really read this message");
            Flash.Info("Info!", "you can read this... if you want.");
            Flash.Error("Error!", "The sky is falling!");

            return View();
        }

    }
}
