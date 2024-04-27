using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UbStandardObjects;
using UbStandardObjects.Helpers;
using static System.Environment;

namespace AmadonStandardLib.Classes
{
    internal class Initializer
    {
        private static Initializer _initializer = new();

        private const string AppName = "AmandonApp";

        // Get Github data
        private GitHelper gitHelper = GitHelper.Instance;


        public static Initializer Instance { get { return _initializer; } }

        private string GetDataFolder()
        {
            string dataFolder = Path.Combine(GetFolderPath(SpecialFolder.CommonApplicationData), AppName);
            Directory.CreateDirectory(dataFolder);
            return dataFolder;
        }

        /// <summary>
        /// Initialize the log for information, wartings and errors
        /// </summary>
        public bool Log()
        {
            try
            {
                StaticObjects.PathLog = Path.Combine(GetDataFolder(), $"{AppName}.log");
                StaticObjects.Logger = new LogMAUI();
                StaticObjects.Logger.Initialize(StaticObjects.PathLog, false);
                StaticObjects.Logger.Info($"Log initialized at {DateTime.Now.ToUniversalTime} ");
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Could not start log", ex);
                return false;
            }
        }

        /// <summary>
        /// Get the parameters for the app
        /// </summary>
        /// <returns></returns>
        public bool Parameters()
        {
            try
            {
                StaticObjects.PathParameters = Path.Combine(GetDataFolder(), $"{AppName}.json");
                if (!File.Exists(StaticObjects.PathParameters))
                {
                    StaticObjects.Logger.Info("Parameters not found, creating a new one: " + StaticObjects.PathParameters);
                }
                StaticObjects.Parameters = ParametersMAUI.Deserialize(StaticObjects.PathParameters);
                StaticObjects.Parameters.ApplicationFolder = System.AppDomain.CurrentDomain.BaseDirectory;
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Could not open/create parameters", ex);
                return false;
            }
        }

        /// <summary>
        /// Start the repository for the several translations
        /// </summary>
        /// <returns></returns>
        public bool StartTubRepository()
        {
            try
            {
                // TUB files (existing translations) folder
                StaticObjects.Parameters.TUB_Files_RepositoryFolder = Path.Combine(GetDataFolder(), "TUB_Files");
                Directory.CreateDirectory(StaticObjects.Parameters.TUB_Files_RepositoryFolder);
                StaticObjects.Logger.Info($"TUB_Files_RepositoryFolder: {StaticObjects.Parameters.TUB_Files_RepositoryFolder}");
                gitHelper.VerifyRepository("https://github.com/Rogreis/TUB_Files.git", StaticObjects.Parameters.TUB_Files_RepositoryFolder);
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Could not start TUB repository", ex);
                return false;
            }
        }

        /// <summary>
        /// Start the edit data repository
        /// </summary>
        /// <returns></returns>
        public bool StartEditRepository()
        {
            try
            {
                // Repository folder for edit language when exists
                StaticObjects.Parameters.EditParagraphsRepositoryFolder = Path.Combine(GetDataFolder(), "Repo");
                Directory.CreateDirectory(StaticObjects.Parameters.TUB_Files_RepositoryFolder);
                StaticObjects.Logger.Info($"EditParagraphsRepositoryFolder: {StaticObjects.Parameters.EditParagraphsRepositoryFolder}");
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Could not start edit data repository", ex);
                return false;
            }
        }



    }
}
