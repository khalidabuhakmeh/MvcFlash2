using System;
using MvcFlash.Core.Messages;

namespace MvcFlash.Core.Extensions
{
    public static class FlashPusherExtensions
    {
        public static MessageBase Success<T>(this IFlashPusher messenger, string title = "", string content = "", string id = "", T data = default(T), string template = "")
        {
            return Push(messenger, Flash.Types.Success, title, content, id, data, template);
        }

        public static MessageBase Success(this IFlashPusher messenger, string title = "", string content = "", string id = "", string template ="")
        {
            return Push(messenger, Flash.Types.Success, title, content, id, template);
        }

        public static MessageBase Warning<T>(this IFlashPusher messenger, string title = "", string content = "", string id = "", T data = default(T), string template = "")
        {
            return Push(messenger, Flash.Types.Warning, title, content, id, data, template);
        }

        public static MessageBase Warning(this IFlashPusher messenger, string title = "", string content = "", string id = "", string template = "")
        {
            return Push(messenger, Flash.Types.Warning, title, content, id, template);
        }

        public static MessageBase Info<T>(this IFlashPusher messenger, string title = "", string content = "", string id = "", T data = default(T), string template = "")
        {
            return Push(messenger, Flash.Types.Info, title, content, id, data, template);
        }

        public static MessageBase Info(this IFlashPusher messenger, string title = "", string content = "", string id = "", string template = "")
        {
            return Push(messenger, Flash.Types.Info, title, content, id, template);
        }

        public static MessageBase Error<T>(this IFlashPusher messenger, string title = "", string content = "", string id = "", T data = default(T), string template = "")
        {
            return Push(messenger, Flash.Types.Error, title, content, id, data, template);
        }

        public static MessageBase Error(this IFlashPusher messenger, string title = "", string content = "", string id = "", string template = "")
        {
            return Push(messenger, Flash.Types.Error, title, content, id, template);
        }

        private static MessageBase Push<T>(IFlashPusher messenger, string type, string title = "", string content = "", string id = "", T data = default(T), string template = "")
        {
            var message = new SimpleMessage
            {
                Title = title,
                Content = content,
                MessageType = type,
                Data = data,
                Template = template
            };

            if (!string.IsNullOrWhiteSpace(id))
                message.Id = id;

            return messenger.Push(message);
        }

        private static MessageBase Push(IFlashPusher messenger, string type, string title = "", string content = "", string id = "", string template = "")
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            var message = new SimpleMessage
            {
                Title = title,
                Content = content,
                MessageType = type,
                Data = null,
                Template = template
            };

            if (!string.IsNullOrWhiteSpace(id))
                message.Id = id;

            return messenger.Push(message);
        }
    }
}
