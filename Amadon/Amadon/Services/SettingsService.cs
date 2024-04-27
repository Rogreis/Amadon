using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;

namespace Amadon.Services
{
    public class SettingsService
    {
        public static Task Store()
        {
            Parameters.Serialize();
            PersistentData.Serialize();
            return Task.CompletedTask;
        }

        public static Task Get()
        {
            StaticObjects.Parameters= Parameters.Deserialize(Parameters.PathParameters);
            return Task.CompletedTask;
        }
    }
}
