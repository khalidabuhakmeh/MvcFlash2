using System.Web.Mvc;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;

namespace MvcFlash.Sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Flash.Instance.Success("Well Done!", "You successfully read this important alert message.");
            Flash.Instance.Warning("Warning!", "You should really read this message");
            Flash.Instance.Info("Info!", "you can read this... if you want.");
            Flash.Instance.Error("Error!", "The sky is falling!");

            return View();
        }
    }
}
