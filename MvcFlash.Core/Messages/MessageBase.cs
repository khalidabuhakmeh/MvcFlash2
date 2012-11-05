using System;

namespace MvcFlash.Core.Messages
{
    public abstract class MessageBase
    {
        protected MessageBase()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string MessageType { get; set; }
    }
}
