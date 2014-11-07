using MvcFlash.Core.Messages;

namespace MvcFlash.Sample.Models
{
    public class CookieFlashModel
    {
        public string Type { get; set; } 
        public string Title { get; set; }
        public string Content { get; set; }

        public SimpleMessage ToSimpleMessage()
        {
            return new SimpleMessage
            {
                MessageType = Type,
                Title = Title ?? "[empty title]",
                Content = Content ?? "Enter a message, don't be so shy!"
            };
        }
    }
}