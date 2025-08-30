using AmadonStandardLib.Helpers;
using Microsoft.Extensions.Logging;

namespace Amadon.Services
{
    public static class InitializationService
    {
        public static bool InitLogger()
        {
            if (StaticObjects.Logger != null)
            {
                return true;
            }
            string logFilePath = DataInitializer.GetFullLogPath();
            FileLoggerProvider fileLoggerProvider = new FileLoggerProvider(logFilePath, LogLevel.Information);
            //builder.Logging.AddProvider(fileLoggerProvider).SetMinimumLevel(LogLevel.Debug);
            StaticObjects.Logger = new Logger(fileLoggerProvider.CreateLogger("Amadon"));
            //PersistentData.SetPath(logFilePath);
            return true;
        }

        public static bool CheckLogger()
        {
            return StaticObjects.Logger != null;
        }

        public static async Task<bool> InitParameters()
        {
            bool ret = DataInitializer.InitParameters();
            await SettingsService.Get();
            return true;
        }

        public static async Task<bool> StoreParameters()
        {
            await SettingsService.Store();
            return true;
        }

        public static async Task<bool> InitTranslationsList()
        {
            bool ret= await DataInitializer.InitTranslationsList();
            return ret;
        }


        public static async Task<bool> InitEachTranslation()
        {
            bool ret = await DataInitializer.InitTranslation();
            return ret;
        }

        public static async Task<bool> InitSubjectIndex()
        {
            bool ret = await DataInitializer.InitSubjectIndex();
            return ret;
        }


    }
}
