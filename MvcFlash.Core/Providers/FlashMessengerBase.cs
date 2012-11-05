using System;
using System.Collections.Generic;
using System.Linq;
using MvcFlash.Core.Messages;

namespace MvcFlash.Core.Providers
{
    public abstract class FlashMessengerBase : IFlashMessenger
    {
        protected abstract IDictionary<string, MessageBase> Messages { get; }

        public virtual MessageBase Push(MessageBase message)
        {
            var key = string.IsNullOrWhiteSpace(message.Id)
                          ? (message.Id = Guid.NewGuid().ToString("N"))
                          : message.Id;

            if (Messages.ContainsKey(key))
                Messages.Remove(key);

            Messages.Add(key, message);

            return message;
        }

        public virtual MessageBase Pop()
        {
            var message = Messages.Values.LastOrDefault();

            if (message != null)
                Messages.Remove(message.Id);

            return message;
        }

        public virtual int Count { get { return Messages.Count; } }
    }
}