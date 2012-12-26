using System;

namespace MvcFlash.Core.Messages
{
    [Serializable]
    public class SimpleMessage : MessageBase
    {
        public object Data { get; set; }
    }
}