using AmadonStandardLib.Classes;
using AmadonStandardLib.UbClasses;
using J2N.Collections.Generic;
using Lucene.Net.Search;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Environment;

namespace AmadonStandardLib.Helpers
{
    public class DataInitializer
    {

        protected const string TubFilesFolder = "TUB_Files";

        protected const string AvailableTranslations = "AvailableTranslations.json";

        protected const string IndexFileName = "Index.zip";

        protected const string FormatTableName = "FormatTable.gz";

        #region Private helper functions
        private static string DataFolder()
        {
            string processName = System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var commonpath = GetDataFiles.GetDataFolder();
            return Path.Combine(commonpath, processName);
        }


        private static string MakeProgramDataFolder(string? fileName = null)
        {
            string folder = DataFolder();
            Directory.CreateDirectory(folder);
            if (fileName != null)
            {
                folder = Path.Combine(folder, fileName);
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        private static string MakeGitHubUrl(string relativeFilePath)
        {
            //return $"https://github.com/Rogreis/TUB_Files/blob/main/{relativeFilePath}";
            return $"https://raw.githubusercontent.com/Rogreis/TUB_Files/main/{relativeFilePath}";
        }

        private static string MakeTranslationFileName(short translationId, string extension = "json")
        {
            return $"TR{translationId:000}.{extension}";
        }

        /// <summary>
        /// Generates the translation full path with the given extension
        /// </summary>
        /// <param name="translationId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static string MakeTranslationFilePath(short translationId, string extension= "json")
        {
            return Path.Combine(StaticObjects.Parameters.TubDataFolder, MakeTranslationFileName(translationId, extension));
        }

        /// <summary>
        /// Initialize the format table used for editing translations
        /// </summary>
        private static async Task<bool> GetFormatTable()
        {
            try
            {
                if (StaticObjects.Book.FormatTableObject == null)
                {
                    string formatTableZippedPath = Path.Combine(StaticObjects.Parameters.TubDataFolder, FormatTableName);
                    if (!GetDataFiles.LocalFileExists(formatTableZippedPath))
                    {
                        string url = MakeGitHubUrl(FormatTableName);
                        await GetDataFiles.DownloadBinaryFile(url, formatTableZippedPath);
                    }
                    string ret = await GetDataFiles.GetStringFromZippedFile(formatTableZippedPath);
                    switch (ret)
                    {
                        case GetDataFiles.FileNotFound:
                            StaticObjects.Logger.Error("Table formatting data was not found.");
                            return false;
                        case GetDataFiles.ErrorGettingFile:
                            StaticObjects.Logger.Error("Error reading format data.");
                            return false;
                        default:
                            StaticObjects.Book.FormatTableObject = new FormatTable(ret);
                            return true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error($"Missing format table. May be you do not have the correct data to use this tool.", ex);
                return false;
            }
        }

        #endregion

        #region Initialization funtions

        public static string GetLocalStorageFolder(string storageName)
        {
            return Path.Combine(MakeProgramDataFolder(), storageName);
        }


        public static string GetFullLogPath()
        {
            Logger.PathLog = Path.Combine(MakeProgramDataFolder(), Logger.FileName);
            return Logger.PathLog;
        }


        /// <summary>
        /// Initialize the parameters object
        /// </summary>
        /// <returns></returns>
        public static bool InitParameters()
        {
            try
            {
                Parameters.PathParameters = Path.Combine(MakeProgramDataFolder(), Parameters.FileName);
                if (!File.Exists(Parameters.PathParameters))
                {
                    LibraryEventsControl.FireSendUserAndLogMessage($"Parameters not found, creating a new one.");
                    StaticObjects.Parameters = new Parameters();
                }
                else
                {
                    LibraryEventsControl.FireSendUserAndLogMessage($"Parameters found, reading from disk.");
                    StaticObjects.Parameters = Parameters.Deserialize(Parameters.PathParameters);
                }


                // Set folders and URLs used
                // This must be set in the parameters
                StaticObjects.Parameters.ApplicationDataFolder = MakeProgramDataFolder();
                Directory.CreateDirectory(StaticObjects.Parameters.ApplicationDataFolder);
                StaticObjects.Parameters.IndexSearchFolders = MakeProgramDataFolder("IndexSearch");
                Directory.CreateDirectory(StaticObjects.Parameters.IndexSearchFolders);
                StaticObjects.Parameters.TubSearchFolders = MakeProgramDataFolder("TubSearch");
                Directory.CreateDirectory(StaticObjects.Parameters.TubSearchFolders);
                StaticObjects.Parameters.TubDataFolder = MakeProgramDataFolder("TUB_Files");
                Directory.CreateDirectory(StaticObjects.Parameters.TubDataFolder);

                LibraryEventsControl.FireSendUserAndLogMessage($"Parameters started {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}");
                Parameters.Serialize();

                return true;
            }
            catch (Exception ex)
            {
                string message = "Could not initialize parameters";
                LibraryEventsControl.FireSendUserAndLogMessage(message, ex);
                return false;
            }
        }

        /// <summary>
        /// Initialize the local list of available translations.
        /// Download from github if not found.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> InitTranslationsList()
        {
            try
            {
                StaticObjects.Book = new Book();
                bool ret = false;

                // First try to get from github
                string localAvailableTranslationsPath = Path.Combine(StaticObjects.Parameters.ApplicationDataFolder, AvailableTranslations);
                string url = MakeGitHubUrl(AvailableTranslations);
                ret = await GetDataFiles.DownloadTextFileAsync(url, localAvailableTranslationsPath);
                if (!ret)
                {
                    StaticObjects.Logger.Error("Could not get Translations list from github.");
                    // if could not get from online, try to use local one
                    if (!GetDataFiles.LocalFileExists(localAvailableTranslationsPath))
                    {
                        StaticObjects.Logger.Error("Could not get Translations list from local too.");
                        return false;
                    }
                }

                string json= await GetDataFiles.GetStringFromLocalFile(localAvailableTranslationsPath);
                switch (json)
                {
                    case GetDataFiles.FileNotFound:
                        StaticObjects.Logger.Error("Translations list data was not found.");
                        return false;
                    case GetDataFiles.ErrorGettingFile:
                        StaticObjects.Logger.Error("Translations list data was not found.");
                        return false;
                    default:
                        StaticObjects.Book.Translations= Translations.DeserializeJson(json);
                        break;
                }
                
                ret= await GetFormatTable();
                return ret;
          }
            catch (Exception ex)
            {
                LibraryEventsControl.FireSendUserAndLogMessage("Could not initialize translations list", ex);
                return false;
            }
        }

        public static async Task<bool> InitSubjectIndex()
        {
            string fileName = "tubIndex_000.gz";
            try
            {
                string localSubjectIndexFilePath = Path.Combine(StaticObjects.Parameters.TubDataFolder, fileName);
                if (!File.Exists(localSubjectIndexFilePath))
                {
                    string url = MakeGitHubUrl(fileName);
                    bool ret = await GetDataFiles.DownloadBinaryFile(url, localSubjectIndexFilePath);
                    if (!ret) return ret;
                }
                string json = await GetDataFiles.GetStringFromZippedFile(localSubjectIndexFilePath);
                switch (json)
                {
                    case GetDataFiles.FileNotFound:
                        StaticObjects.Logger.Error($"Repository has no subject index zipped: {fileName}");
                        return false;
                    case GetDataFiles.ErrorGettingFile:
                        StaticObjects.Logger.Error("Error getting subject index.");
                        return false;
                    default:
                        return true;
                }
            }
            catch (Exception ex)
            {
                LibraryEventsControl.FireSendUserAndLogMessage($"Could not initialize subject indez {fileName}", ex);
                return false;
            }
        }

        /// <summary>
        /// Initialize all translations maked as to be shown by the user
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> InitTranslation()
        {
            short currentTranslationId = -1;
            try
            {
                List<short> initTranslations = new List<short>(StaticObjects.Parameters.TranslationsToShowId);
                initTranslations.Add(0); // add the English that is not optional
                foreach (short translationId in initTranslations)
                {
                    currentTranslationId= translationId;
                    LibraryEventsControl.FireSendUserAndLogMessage($"Initializing translation {translationId}");
                    Translation trans = StaticObjects.Book.Translations.Find(t => t.LanguageID == translationId);
                    if (trans != null)
                    {
                        string localTranslationPath = MakeTranslationFilePath(translationId, "gz");

                        if (!GetDataFiles.LocalFileExists(localTranslationPath))
                        {
                            string url = MakeGitHubUrl(MakeTranslationFileName(translationId, "gz"));
                            bool ret = await GetDataFiles.DownloadBinaryFile(url, localTranslationPath);
                            if (!ret) return ret;
                        }

                        string hash = GetDataFiles.CalculateMD5(localTranslationPath);
                        if (trans.Hash != hash)
                        {
                            string url = MakeGitHubUrl(MakeTranslationFileName(translationId, "gz"));
                            bool ret = await GetDataFiles.DownloadBinaryFile(url, localTranslationPath);
                            if (!ret) return ret;
                        }

                        string json = await GetDataFiles.GetStringFromZippedFile(localTranslationPath);
                        switch (json)
                        {
                            case GetDataFiles.FileNotFound:
                                StaticObjects.Logger.Error($"Non existing translation: {translationId}");
                                return false;
                            case GetDataFiles.ErrorGettingFile:
                                StaticObjects.Logger.Error("Error reading translation data.");
                                return false;
                            default:
                                trans.GetData(json);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibraryEventsControl.FireSendUserAndLogMessage($"Could not initialize translation {currentTranslationId}", ex);
                return false;
            }
            return true;
        }
        #endregion

    }
}
