using System;
using MvcFlash.Core.Messages;

namespace MvcFlash.Core.Extensions
{
    public static class FlashPusherExtensions
    {
        public static MessageBase Success<T>(this IFlashPusher messenger, string title = "", string content = "", string id = "", T data = default(T))
        {
            return Push(messenger, Flash.Types.Success, title, content, id, data);
        }
        public static MessageBase Success(this IFlashPusher messenger, string title = "", string content = "", string id = "")
        {
            return Push(messenger, Flash.Types.Success, title, content, id);
        }

        public static MessageBase Warning<T>(this IFlashPusher messenger, string title = "", string content = "", string id = "", T data = default(T))
        {
            return Push(messenger, Flash.Types.Warning, title, content, id, data);
        }
        public static MessageBase Warning(this IFlashPusher messenger, string title = "", string content = "", string id = "")
        {
            return Push(messenger, Flash.Types.Warning, title, content, id);
        }

        public static MessageBase Info<T>(this IFlashPusher messenger, string title = "", string content = "", string id = "", T data = default(T))
        {
            return Push(messenger, Flash.Types.Info, title, content, id, data);
        }
        public static MessageBase Info(this IFlashPusher messenger, string title = "", string content = "", string id = "")
        {
            return Push(messenger, Flash.Types.Info, title, content, id);
        }

        public static MessageBase Error<T>(this IFlashPusher messenger, string title = "", string content = "", string id = "", T data = default(T))
        {
            return Push(messenger, Flash.Types.Error, title, content, id, data);
        }
        public static MessageBase Error(this IFlashPusher messenger, string title = "", string content = "", string id = "")
        {
            return Push(messenger, Flash.Types.Error, title, content, id);
        }

        private static MessageBase Push<T>(IFlashPusher messenger, string type, string title = "", string content = "", string id = "", T data = default(T))
        {
            var message = new SimpleMessage<T>
            {
                Title = title,
                Content = content,
                MessageType = type,
                Data = data
            };

            if (!string.IsNullOrWhiteSpace(id))
                message.Id = id;

            return messenger.Push(message);
        }
        private static MessageBase Push(IFlashPusher messenger, string type, string title = "", string content = "", string id = "")
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            var message = new SimpleMessage<object>
            {
                Title = title,
                Content = content,
                MessageType = type,
                Data = null
            };

            if (!string.IsNullOrWhiteSpace(id))
                message.Id = id;

            return messenger.Push(message);
        }
    }
}
