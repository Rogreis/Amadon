using AmadonStandardLib.Data;
using AmadonStandardLib.Helpers;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AmadonStandardLib.UbClasses
{
    public class GetDataFiles
    {






        /// <summary>
        /// Get public data from github
        /// <see href="https://raw.githubusercontent.com/Rogreis/TUB_Files/main"/>
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <param name="fileName"></param>
        /// <param name="isZip"></param>
        /// <returns></returns>
        protected byte[] GetGitHubBinaryFile(string fileName, bool isZip = false)
        {
            try
            {
                // https://raw.githubusercontent.com/Rogreis/TUB_Files/main/UbHelpTextControl.xml
                StaticObjects.Logger.Info("Downloading files from github.");
                string url = isZip ? $"https://github.com/Rogreis/TUB_Files/raw/main/{fileName}" :
                    $"https://raw.githubusercontent.com/Rogreis/TUB_Files/main/{fileName}";
                StaticObjects.Logger.Info("Url: " + url);

                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("a", "a");
                    byte[] bytes = wc.DownloadData(url);
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                string message = $"Error getting file {fileName}: {ex.Message}. May be you do not have the correct data to use this tool.";
                StaticObjects.Logger.Error(message, ex);
                return null;
            }
        }


        #region Load & Store Annotations
        protected void StoreJsonAnnotations(TOC_Entry entry, string jsonAnnotations)
        {
            string path = TranslationAnnotationsJsonFilePath(entry.TranslationId);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (string.IsNullOrWhiteSpace(jsonAnnotations))
            {
                return;
            }
            File.WriteAllText(path, jsonAnnotations);
        }

        /// <summary>
        /// Loads a list of all TOC/Annotation done for a paper
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        protected string LoadJsonAnnotations(short translationId)
        {
            string pathAnnotationsFile = TranslationAnnotationsJsonFilePath(translationId);
            if (File.Exists(pathAnnotationsFile))
            {
                return File.ReadAllText(pathAnnotationsFile);
            }
            return null;
        }

        #endregion



        #region File path creation
        /// <summary>
        /// Generates the control file full path
        /// </summary>
        /// <returns></returns>
        protected string ControlFilePath()
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, ControlFileName);
        }

        /// <summary>
        /// Generates the translation full path
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        protected string TranslationFilePath(short translationId)
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, $"TR{translationId:000}.gz");
        }


        /// <summary>
        /// Generates the translation full path
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        protected string TranslationJsonFilePath(short translationId)
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, $"TR{translationId:000}.json");
        }

        /// <summary>
        /// Generates the translation full path
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        protected string TranslationAnnotationsJsonFilePath(short translationId)
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, $"{translationAnnotationsFileName}_{translationId:000}.json");
        }

        protected string ParagraphAnnotationsJsonFilePath(short translationId)
        {
            return Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, $"{paragraphAnnotationsFileName}_{translationId:000}.json");
        }

        #endregion


        #region Load & Store Annotations
        ///// <summary>
        ///// Create a json string from the annotations list and store
        ///// </summary>
        ///// <param name="entry"></param>
        ///// <param name="annotations"></param>
        //public void StoreAnnotations(TOC_Entry entry, List<UbAnnotationsStoreData> annotations)
        //{
        //    string jsonAnnotations = annotations.Count == 0 ? null : StaticObjects.Serialize<List<UbAnnotationsStoreData>>(annotations);
        //    StoreJsonAnnotations(entry, jsonAnnotations);
        //}

        ///// <summary>
        ///// Loads a list of all TOC/Annotation done for a paper
        ///// </summary>
        ///// <param name="translationId"></param>
        ///// <returns></returns>
        //public List<UbAnnotationsStoreData> LoadAnnotations(short translationId)
        //{
        //    string jsonAnnotations = LoadJsonAnnotations(translationId);
        //    if (!string.IsNullOrWhiteSpace(jsonAnnotations))
        //    {
        //        return (StaticObjects.DeserializeObject<List<UbAnnotationsStoreDataCore>>(jsonAnnotations)).ToList<UbAnnotationsStoreData>();
        //    }
        //    // Return empty list
        //    return new List<UbAnnotationsStoreDataCore>().ToList<UbAnnotationsStoreData>();
        //}
        #endregion



    }
}
