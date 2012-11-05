using System.Web.Mvc;
using MvcFlash.Core.Extensions;
using MvcFlash.Sample.Models;

namespace MvcFlash.Sample.Controllers
{
    public class TricksController : ApplicationController
    {
        public ActionResult Index()
        {
            Flash.Success("Hello Kitty!", "hey this is cool!",
                          template: "ImageMessage",
                          data: new FlashImage
                          {
                              Alt = "this is a cute kitty",
                              Link = "~/content/images/cat.jpg"
                          });

            Flash.Warning("Oh No!", "We couldn't find the page you were looking for. Try these",
                          template: "HelpfulLinks",
                          data: new RelatedPages
                                    {
                                        {"http://www.google.com"},
                                        {"http://www.yahoo.com"},
                                        {"http://www.github.com"}
                                    }
                );


            return RedirectToAction("Redirected");
        }


        public ActionResult Redirected()
        {
            return View("index");
        }
    }
}
