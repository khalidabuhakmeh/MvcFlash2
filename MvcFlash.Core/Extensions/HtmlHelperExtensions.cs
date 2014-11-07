using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcFlash.Core.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Flash<TModel>(this HtmlHelper<TModel> helper)
        {
            var popper = DependencyResolver.Current.GetService<IFlashMessenger>()
                ?? Core.Flash.Instance;

            return Flash(helper, popper);
        }        
        
        public static MvcHtmlString Flash<TModel>(this HtmlHelper<TModel> helper, IFlashMessenger flashMessenger)
        {
            var builder = new StringBuilder();

            while (flashMessenger.Count > 0)
            {
                var message = flashMessenger.Pop();
                builder.AppendLine(string.IsNullOrWhiteSpace(message.Template)
                                       ? helper.DisplayFor(m => message).ToString()
                                       : helper.DisplayFor(m => message, message.Template).ToString());
            }

            return MvcHtmlString.Create(builder.ToString());
        }



        public static T Cast<T>(this object value)
        {
            if (value == null)
                return default(T);

            return (T) value;
        }
    }
}
