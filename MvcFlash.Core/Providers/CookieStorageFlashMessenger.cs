using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using MvcFlash.Core.Messages;

namespace MvcFlash.Core.Providers
{
    public class CookieStorageFlashMessenger : IFlashMessenger
    {
        private readonly ICookieProvider _cookieProvider;
        private readonly string _cookieName;

        private IDictionary<string, SimpleMessage> _messages;

        public CookieStorageFlashMessenger(ICookieProvider cookieProvider, string cookieName = "__FlashCookie__")
        {
            _cookieProvider = cookieProvider;
            _cookieName = cookieName;
        }

        public MessageBase Push(MessageBase message)
        {
            if (message == null)
                throw new ArgumentNullException("message", "message cannot be null");

            var simpleMessage = message as SimpleMessage;

            if(simpleMessage == null)
                throw new ArgumentException("Cookie Flash Storage only allows instances of SimpleMessage to be used!", "message");
            
            EnsureDataLoadedFromRequestCookie();

            _messages[message.Id] = (SimpleMessage)message;

            SaveExistingMessagesToResponseCookie();

            return message;
        }

        public MessageBase Pop()
        {
            EnsureDataLoadedFromRequestCookie();

            var messageKey = _messages.Keys.LastOrDefault();

            if (messageKey == null)
                return null;

            var message = _messages[messageKey];
            _messages.Remove(messageKey);

            SaveExistingMessagesToResponseCookie();
            
            return message;
        }

        public int Count
        {
            get
            {
                EnsureDataLoadedFromRequestCookie();
                return _messages.Count;
            }
        }

        public void Clear()
        {
            _messages = new Dictionary<string, SimpleMessage>();

            var eraseCookie = new HttpCookie(_cookieName)
            {
                Expires = DateTime.Now.AddYears(-1)
            };

            _cookieProvider.SetResponseCookie(eraseCookie);
        }


        private void EnsureDataLoadedFromRequestCookie()
        {
            if(_messages != null)
                return;

            var requestCookie = _cookieProvider.GetRequestCookie(_cookieName);

            if (requestCookie == null)
            {
                _messages = new Dictionary<string, SimpleMessage>();
                return;                
            }

            var serializer = new JavaScriptSerializer();

            _messages = serializer.Deserialize<Dictionary<string, SimpleMessage>>(requestCookie.Value);
        }

        private void SaveExistingMessagesToResponseCookie()
        {
            var value = new JavaScriptSerializer().Serialize(_messages);

            var responseCookie = new HttpCookie(_cookieName, value);

            _cookieProvider.SetResponseCookie(responseCookie);
        }
    }

    public interface ICookieProvider
    {
        HttpCookie GetRequestCookie(string cookieName);
        void SetResponseCookie(HttpCookie cookie);
    }

    public class HttpCookieProvider : ICookieProvider
    {
        public HttpCookie GetRequestCookie(string cookieName)
        {
            return HttpContext.Current.Request.Cookies[cookieName];
        }

        public void SetResponseCookie(HttpCookie cookie)
        {
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
    }
}