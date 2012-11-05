namespace MvcFlash.Core.Messages
{
    public class SimpleMessage<T> : MessageBase
    {
        public T Data { get; set; }
    }
}