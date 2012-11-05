using System;
using System.Linq;

namespace MvcFlash.Core
{
    public static class MvcFlash
    {
        public static object Sync = new object();
        private static IFlashMessenger _instance;
        internal const string DefaultSuccess = "success";
        internal const string DefaultError = "error";
        internal const string DefaultInfo = "info";
        internal const string DefaulWarning = "warning";

        public static class Types
        {
            public static string Success = DefaultSuccess;
            public static string Error = DefaultError;
            public static string Info = DefaultInfo;
            public static string Warning = DefaulWarning;
        }

        public static IFlashMessenger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Sync)
                    {
                        if (_instance == null)
                            Initialize();
                    }
                }

                return _instance;
            }
        }

        public static void Initialize(FlashSettings settings = null)
        {
            if (settings == null)
                settings = FlashSettings.Default;

            if (settings.Types.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("one or more types are empty, please correct", "settings");

            lock (Sync)
            {
                _instance = settings.Messenger;
                Types.Success = settings.Success;
                Types.Error = settings.Error;
                Types.Info = settings.Info;
                Types.Warning = settings.Warning;
            }
        }

        public static void Reset()
        {
            _instance = null;
            Types.Success = DefaultSuccess;
            Types.Error = DefaultError;
            Types.Info = DefaultInfo;
            Types.Warning = DefaulWarning;
        }
    }
}
