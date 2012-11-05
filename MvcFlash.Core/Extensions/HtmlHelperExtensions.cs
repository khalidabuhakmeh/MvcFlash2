using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcFlash.Core.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static void Flash<TModel>(this HtmlHelper<TModel> helper)
        {
            var popper = DependencyResolver.Current.GetService<IFlashMessenger>()
                ?? MvcFlash.Instance;

            while (popper.Count > 0)
            {
                var message = popper.Pop();
                helper.DisplayFor(m => message);
            }
        }
    }
}
