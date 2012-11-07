using System.Web.Mvc;

namespace MvcFlash.Core.Filters
{
    public class ClearFlashOnAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                Flash.Instance.Clear();
            }
        }
    }
}
