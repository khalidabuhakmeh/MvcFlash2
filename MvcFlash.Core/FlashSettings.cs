using System;
using MvcFlash.Core.Providers;

namespace MvcFlash.Core
{
    public class FlashSettings
    {
        static FlashSettings()
        {
            Default = () => new FlashSettings { Messenger =  new HttpSessionFlashMessenger() };
        }

        public FlashSettings()
        {
            Success = Flash.DefaultSuccess;
            Error = Flash.DefaultError;
            Info = Flash.DefaultInfo;
            Warning = Flash.DefaulWarning;
        }

        public string Success { get; set; }
        public string Error { get; set; }
        public string Info { get; set; }
        public string Warning { get; set; }
        public IFlashMessenger Messenger { get; set; }

        public string[] Types
        {
            get
            {
                return new[] { Success, Error, Info, Warning };
            }
        }

        public static Func<FlashSettings> Default { get; set; }
    }
}