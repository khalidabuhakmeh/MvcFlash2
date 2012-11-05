using MvcFlash.Core.Providers;

namespace MvcFlash.Core
{
    public class FlashSettings
    {
        public FlashSettings()
        {
            Success = MvcFlash.DefaultSuccess;
            Error = MvcFlash.DefaultError;
            Info = MvcFlash.DefaultInfo;
            Warning = MvcFlash.DefaulWarning;
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

        public static FlashSettings Default
        {
            get
            {
                return new FlashSettings { Messenger = new HttpSessionFlashMessenger() };
            }
        }
    }
}