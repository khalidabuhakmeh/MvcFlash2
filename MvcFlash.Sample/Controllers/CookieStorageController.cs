using System.Web.Mvc;
using MvcFlash.Core;
using MvcFlash.Core.Providers;
using MvcFlash.Sample.Models;

namespace MvcFlash.Sample.Controllers
{
    public class CookieStorageController : Controller
    {
        private readonly IFlashMessenger _cookieFlashMessenger = 
            new CookieStorageFlashMessenger(new HttpCookieProvider());

        public ActionResult Index()
        {
            return View(_cookieFlashMessenger);
        }

        public ActionResult SetFlashAndRedirect(CookieFlashModel model)
        {
            _cookieFlashMessenger.Push(model.ToSimpleMessage());
            
            return RedirectToAction("Index");
        }
    }
}