using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

// credit for these test classes goes to 
// http://offroadcoder.com/index.php/2011/08/unit-testing-asp-net-html-helpers-with-simple-fakes/
// I usually use Moq, but thought it would be more smelly than using this
namespace MvcFlash.Tests.Fakes
{
	public class FakeControllerContext : ControllerContext
	{
		public FakeControllerContext(ControllerBase controller)
			: this(controller, new RouteData(), String.Empty, null, null, null, null, null, null)
		{
		}

		public FakeControllerContext(ControllerBase controller, NameValueCollection formParams)
			: this(controller, new RouteData(), String.Empty, null, null, formParams, null, null, null)
		{
		}

		public FakeControllerContext(ControllerBase controller, string userName)
			: this(controller, new RouteData(), String.Empty, userName, null, null, null, null, null)
		{
		}

		public FakeControllerContext(ControllerBase controller, HttpCookieCollection cookies)
			: this(controller, new RouteData(), String.Empty, null, null, null, null, cookies, null)
		{
		}

		public FakeControllerContext(ControllerBase controller, RouteData routeData)
			: this(controller, routeData, String.Empty, null, null, null, null, null, null)
		{
		}

		public FakeControllerContext(ControllerBase controller, SessionStateItemCollection sessionItems)
			: this(controller, new RouteData(), String.Empty, null, null, null, null, null, sessionItems)
		{
		}

		public FakeControllerContext(ControllerBase controller, NameValueCollection formParams, NameValueCollection queryStringParams)
			: this(controller, new RouteData(), String.Empty, null, null, formParams, queryStringParams, null, null)
		{
		}

		public FakeControllerContext(ControllerBase controller, string userName, NameValueCollection formParams)
			: this(controller, new RouteData(), String.Empty, userName, null, formParams, null, null, null)
		{
		}

		public FakeControllerContext(ControllerBase controller, string userName, string[] roles)
			: this(controller, new RouteData(), String.Empty, userName, roles, null, null, null, null)
		{
		}

		public FakeControllerContext(ControllerBase controller, RouteData routeData, string appRelativeUrl, string userName, string[] roles, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems)
			: base(new FakeHttpContext(null, appRelativeUrl, "GET", new FakePrincipal(new FakeIdentity(userName), roles), formParams, queryStringParams, cookies, sessionItems), routeData, controller)
		{
		}
	}

    public class FakeFormContext : FormContext
    {
    }

    public class FakeHtmlHelper<TModel> : HtmlHelper<TModel>
    {
        public FakeHtmlHelper(ControllerContext controllerContext)
            : this(controllerContext, "index", new RouteData())
        {
        }

        public FakeHtmlHelper(ControllerContext controllerContext, RouteData routeData)
            : this(controllerContext, "index", routeData)
        {
        }

        public FakeHtmlHelper(ControllerContext controllerContext, string viewName, RouteData routeData)
            : base(new FakeViewContext(controllerContext, viewName, routeData), new FakeViewDataContainer())
        {
        }
    }

    public class FakeHttpContext : HttpContextBase
    {
        // Fields
        private readonly string _appRelativeUrl;
        private readonly HttpCookieCollection _cookies;
        private readonly NameValueCollection _formParams;
        private readonly string _httpMethod;
        private readonly HttpRequestBase _httpRequest;
        private readonly HttpResponseBase _httpResponse;
        private bool _isAuthenticated;
        private readonly NameValueCollection _queryStringParams;
        private readonly SessionStateItemCollection _sessionItems;
        private readonly Uri _url;
        private readonly IPrincipal _principal;

        // Methods
        public FakeHttpContext()
            : this(null, "~/", "GET", null, null, null, null, null)
        {
            this._principal = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);
        }

        public FakeHttpContext(string appRelativeUrl)
            : this(null, appRelativeUrl, "GET", null, null, null, null, null)
        {
        }

        public FakeHttpContext(string appRelativeUrl, string httpMethod)
            : this(null, appRelativeUrl, httpMethod, null, null, null, null, null)
        {
        }

        public FakeHttpContext(Uri url, string appRelativeUrl)
            : this(url, appRelativeUrl, "GET", null, null, null, null, null)
        {
        }

        public FakeHttpContext(Uri url, string appRelativeUrl, IPrincipal principal)
            : this(url, appRelativeUrl, "GET", principal, null, null, null, null)
        {
        }

        public FakeHttpContext(Uri url, string appRelativeUrl, string httpMethod, IPrincipal principal, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems)
        {
            this._isAuthenticated = false;
            this._url = url;
            this._appRelativeUrl = appRelativeUrl;
            this._httpMethod = httpMethod;
            this._formParams = formParams;
            this._queryStringParams = queryStringParams;
            this._cookies = cookies;
            this._sessionItems = sessionItems;
            if (principal != null)
            {
                this._principal = principal;
                this._isAuthenticated = principal.Identity.IsAuthenticated;
            }
            this._httpRequest = new FakeHttpRequest(this._url, this._appRelativeUrl, this._httpMethod, this._isAuthenticated, this._formParams, this._queryStringParams, this._cookies);
            this._httpResponse = new FakeHttpResponse();
        }

        // Properties
        public override HttpRequestBase Request
        {
            get
            {
                return this._httpRequest;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return this._httpResponse;
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                return new FakeHttpSessionState(this._sessionItems);
            }
        }

        public override IPrincipal User
        {
            get
            {
                return _principal;
            }
            set
            {
                base.User = value;
            }
        }

        private Dictionary<object, object> _items = new Dictionary<object, object>();

        public override IDictionary Items { get { return _items; } }
    }

    public class FakeHttpRequest : HttpRequestBase
    {
        // Fields
        private readonly string _appRelativeUrl;
        private readonly HttpCookieCollection _cookies;
        private readonly NameValueCollection _formParams;
        private readonly string _httpMethod;
        private readonly bool _isAuthenticated = false;
        private readonly NameValueCollection _queryStringParams;
        private readonly Uri _url;

        // Methods
        public FakeHttpRequest(Uri url, string appRelativeUrl, string httpMethod, bool isAuthenticated, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies)
        {
            if (!(string.IsNullOrEmpty(appRelativeUrl) || appRelativeUrl.StartsWith("~")))
            {
                throw new Exception("appRelativeUrl must start with ~");
            }
            this._url = url;
            this._appRelativeUrl = appRelativeUrl;
            this._httpMethod = httpMethod;
            this._isAuthenticated = isAuthenticated;
            this._formParams = formParams;
            this._queryStringParams = queryStringParams;
            this._cookies = cookies;
        }

        // Properties
        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return this._appRelativeUrl;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return this._cookies;
            }
        }

        public override NameValueCollection Form
        {
            get
            {
                return this._formParams;
            }
        }

        public override string HttpMethod
        {
            get
            {
                return this._httpMethod;
            }
        }

        public override bool IsAuthenticated
        {
            get
            {
                return this._isAuthenticated;
            }
        }

        public override bool IsLocal
        {
            get
            {
                return ((this.Url.Host.ToLower() == "localhost") || (this.Url.Host == "127.0.01"));
            }
        }

        public override string PathInfo
        {
            get
            {
                return String.Empty;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return this._queryStringParams;
            }
        }

        public override Uri Url
        {
            get
            {
                return this._url;
            }
        }
    }

    public class FakeHttpResponse : HttpResponseBase
    {
        // Fields
        private StringBuilder _sb = new StringBuilder();
        private int _statusCode;
        private StringWriter _sw;

        // Methods
        public FakeHttpResponse()
        {
            this._sw = new StringWriter(this._sb);
        }

        public override void Clear()
        {
            this._sb = new StringBuilder();
            this._sw = new StringWriter();
        }

        public override string ToString()
        {
            return this._sb.ToString();
        }

        public override void Write(string s)
        {
            this._sb.Append(s);
        }

        // Properties
        public override HttpCookieCollection Cookies
        {
            get
            {
                return base.Cookies;
            }
        }

        public override TextWriter Output
        {
            get
            {
                return this._sw;
            }
        }

        public override int StatusCode
        {
            get
            {
                return this._statusCode;
            }
            set
            {
                this._statusCode = value;
            }
        }

        public override string StatusDescription { get; set; }
    }

    public class FakeHttpSessionState : HttpSessionStateBase
    {
        // Fields
        private readonly SessionStateItemCollection _sessionItems;

        // Methods
        public FakeHttpSessionState(SessionStateItemCollection sessionItems)
        {
            this._sessionItems = sessionItems;
        }

        public override void Add(string name, object value)
        {
            this._sessionItems[name] = value;
        }

        public override IEnumerator GetEnumerator()
        {
            return this._sessionItems.GetEnumerator();
        }

        public override void Remove(string name)
        {
            this._sessionItems.Remove(name);
        }

        // Properties
        public override int Count
        {
            get
            {
                return this._sessionItems.Count;
            }
        }

        public override object this[string name]
        {
            get
            {
                return this._sessionItems[name];
            }
            set
            {
                this._sessionItems[name] = value;
            }
        }

        public override object this[int index]
        {
            get
            {
                return this._sessionItems[index];
            }
            set
            {
                this._sessionItems[index] = value;
            }
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return this._sessionItems.Keys;
            }
        }
    }

    public class FakeIdentity : IIdentity
    {
        // Fields
        private readonly string _name;

        // Methods
        public FakeIdentity(string userName)
        {
            this._name = userName;
        }

        // Properties
        public string AuthenticationType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(this._name);
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }

    public class FakePrincipal : IPrincipal
    {
        // Fields
        private readonly IIdentity _identity;
        private readonly string[] _roles;

        // Methods
        public FakePrincipal(IIdentity identity, string[] roles)
        {
            this._identity = identity;
            this._roles = roles;
        }

        public bool IsInRole(string role)
        {
            if (this._roles == null)
            {
                return false;
            }
            return this._roles.ToList().Contains(role);
        }

        // Properties
        public IIdentity Identity
        {
            get
            {
                return this._identity;
            }
        }
    }

    public class FakeViewContext : ViewContext
    {
        public StringBuilder WriterOutput { get; set; }
        public TextWriter TextWriter { get; set; }

        public FakeViewContext(ControllerContext controllerContext, string viewName, RouteData routeData)
        {
            WriterOutput = new StringBuilder();

            this.Controller = controllerContext.Controller;
            this.View = new RazorView(controllerContext, viewName, "Layout", false, new string[] { });
            this.ViewData = new ViewDataDictionary();
            this.TempData = new TempDataDictionary();
            this.Writer = new StringWriter(WriterOutput);
            this.RouteData = routeData;
        }
    }

    public class FakeViewDataContainer : IViewDataContainer
    {
        private ViewDataDictionary _viewData = new ViewDataDictionary();

        public ViewDataDictionary ViewData { get { return _viewData; } set { _viewData = value; } }
    }

    public class FakeViewEngine : IViewEngine
    {
        public List<string> MethodLog = new List<string>();

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            MethodLog.Add(String.Format("FindPartialView(controllerContext, {0}, {1})", partialViewName, useCache.ToString()));
            return new ViewEngineResult(new List<string>());
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            MethodLog.Add(String.Format("FindView(controllerContext, {0}, {1}, {2})", viewName, masterName, useCache.ToString()));
            return new ViewEngineResult(new List<string>());
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            MethodLog.Add("ReleaseView(controllerContext, view)");
        }
    }
}
