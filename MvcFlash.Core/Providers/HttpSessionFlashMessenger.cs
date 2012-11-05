using System;
using System.Collections.Generic;
using System.Web;
using MvcFlash.Core.Messages;

namespace MvcFlash.Core.Providers
{
    public class HttpSessionFlashMessenger : FlashMessengerBase
    {
        private readonly HttpContextBase _httpContext;
        private static readonly object _sync = new object();
        private const string MvcflashMessages = "___mvcflash_messages";

        public HttpSessionFlashMessenger()
            :this(new HttpContextWrapper(HttpContext.Current))
        {}

        private HttpSessionFlashMessenger(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentException("http context is required to use the HttpSessionFlashMessenger");

            _httpContext = httpContext;
        }

        protected override IDictionary<string, MessageBase> Messages
        {
            get
            {
                if (_httpContext.Session[MvcflashMessages] as IDictionary<string, MessageBase> == null)
                {
                    lock(_sync)
                    {
                        if (_httpContext.Session[MvcflashMessages] as IDictionary<string, MessageBase> == null)
                        {
                            _httpContext.Session[MvcflashMessages] = new Dictionary<string, MessageBase>();
                        }
                    }
                }
                return _httpContext.Session[MvcflashMessages] as IDictionary<string, MessageBase>;
            }
        }
    }
}
