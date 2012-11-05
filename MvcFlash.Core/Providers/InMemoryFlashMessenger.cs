using System.Collections.Generic;
using MvcFlash.Core.Messages;

namespace MvcFlash.Core.Providers
{
    public class InMemoryFlashMessenger : FlashMessengerBase
    {
        private readonly Dictionary<string, MessageBase> _messages;
        protected override IDictionary<string, MessageBase> Messages { get { return _messages; } }

        public InMemoryFlashMessenger()
        {
            _messages = new Dictionary<string, MessageBase>();
        }
    }
}
