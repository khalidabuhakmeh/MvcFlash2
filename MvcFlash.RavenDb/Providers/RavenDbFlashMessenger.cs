using System;
using System.Collections.Generic;
using System.Web;
using MvcFlash.Core.Messages;
using Raven.Client;
using Raven.Client.Document;
using System.Linq;

namespace MvcFlash.Core.Providers
{
    public class RavenDbFlashMessenger : FlashMessengerBase
    {
        private readonly IDocumentStore _db;
        private readonly HttpContextBase _context;
        private MvcFlashRavenDbContainer _container;
        private const string CookieKey = "mvcflash.ravendb";

        public RavenDbFlashMessenger()
        {
            _container = new MvcFlashRavenDbContainer();
            _db = new DocumentStore { ConnectionStringName = "MvcFlash.RavenDb" }.Initialize();
        }

        /// <summary>
        /// You should use this and pass in your documentstore, the default
        /// constructore will initialize the datastore everytime the class is
        /// created which is not optimal.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="context"></param>
        public RavenDbFlashMessenger(IDocumentStore db, HttpContextBase context)
        {
            _db = db;
            _context = context;
        }

        protected override IDictionary<string, MessageBase> Messages
        {
            get
            {
                var context = _context ?? new HttpContextWrapper(HttpContext.Current);
                var cookieExists = context.Request.Cookies.AllKeys.Contains(CookieKey);

                if (cookieExists)
                {
                    using (var session = _db.OpenSession())
                        _container = session.Load<MvcFlashRavenDbContainer>(context.Request.Cookies[CookieKey].Value);

                    if (_container == null)
                    {
                        _container = new MvcFlashRavenDbContainer { Id = Guid.NewGuid().ToString("N") };
                        Save();
                    }
                }
                else
                {
                    _container = new MvcFlashRavenDbContainer() { Id = Guid.NewGuid().ToString("N") };
                    context.Response.Cookies.Add(new HttpCookie(CookieKey, _container.Id));
                    Save();
                }

                return _container.Messages;
            }
        }

        public override MessageBase Push(MessageBase message)
        {
            var item = base.Push(message);
            // flush to ravendb
            Save();
            return item;
        }

        public override MessageBase Pop()
        {
            var item = base.Pop();
            Save();
            return item;
        }

        public override int Count { get { return _container.Messages.Count; } }

        public void Clear()
        {
            _container.Messages.Clear();
            Save();
        }

        protected virtual void Save()
        {
            // flush to ravendb
            using (var session = _db.OpenSession())
            {
                session.Store(_container);
                session.SaveChanges();
            }
        }

        public class MvcFlashRavenDbContainer
        {
            public MvcFlashRavenDbContainer()
            {
                Messages = new Dictionary<string, MessageBase>();
            }

            public string Id { get; set; }
            public IDictionary<string, MessageBase> Messages { get; set; }
        }
    }
}
