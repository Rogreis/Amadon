using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AmadonStandardLib.Classes
{

    public enum TextShowOption
    {
        LeftOnly = 0,
        LeftRight = 1,
        LeftMiddleRight = 2,
        LeftMiddle = 3
    }

    public enum TranslatioForTocSearch
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }


    public class Parameters
    {
        public const string FileName = "AmadonParameters.json";

        public static string? PathParameters { get; set; }

        // Controls if the choise for translations to be shown are done
        public bool TranslationsChoiceDone { get; set; } = false;

        // Last notes file used
        public string NotesFile { get; set; } = "";

        /// <summary>
        /// Last position in the text, default for first paragraph
        /// </summary>
        public TOC_Entry Entry { get; set; } = new TOC_Entry(0, 0, 1, 0, 0, 0);

        /// <summary>
        /// Last controls used
        /// </summary>
        public string LastLeftControl { get; set; } = "toc";

        public short LanguageIDLeftTranslation { get; set; } = 0;

        public short LanguageIDMiddleTranslation { get; set; } = -1;

        public short LanguageIDRightTranslation { get; set; } = 34;

        public TranslatioForTocSearch TranslationForTableOfContents { get; set; } = TranslatioForTocSearch.Right;

        // Show pages options
        public bool ShowMiddle { get; set; } = false;
        public bool ShowRight { get; set; } = true;

        public List<short> TranslationsToShowId { get; set; } = new List<short>();

        [JsonIgnore]
        public bool AppInitialized = false;

        public bool UseDarkThemme { get; set; } = true;

        public bool UseSerifFont { get; set; } = true;

        public int SearchPageSize { get; set; } = 20;

        public bool ShowParagraphIdentification { get; set; } = true;

        /// <summary>
        /// Max items stored for  search and index text
        /// </summary>
        public int MaxExpressionsStored { get; set; } = 50;

        public string FontFamilyInfo { get; set; } = "Verdana";

        public double FontSizeInfo { get; set; } = 18;


        /// <summary>
        /// Current data folder
        /// </summary>
        public string ApplicationDataFolder { get; set; } = "";

        /// <summary>
        /// Folder to store local lucene index search data
        /// </summary>
        public string IndexSearchFolders { get; set; } = "";

        /// <summary>
        /// Folder to store local lucene TUB search data
        /// </summary>
        public string TubSearchFolders { get; set; } = "";

        /// <summary>
        /// Git associated repository folder
        /// </summary>
        public string TubDataFolder { get; set; } = "";


        public int FontSize { get; set; } = 18;

        /// <summary>
        /// Serialize the parameters instance
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pathParameters"></param>
        public static void Serialize()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                };
                if (StaticObjects.Parameters != null)
                {
                    var jsonString = JsonSerializer.Serialize<Parameters>(StaticObjects.Parameters, options);
                    File.WriteAllText(Parameters.PathParameters, jsonString);
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Serialize Parameters failure", ex);
            }
        }

        /// <summary>
        /// Deserialize the parameters instance
        /// </summary>
        /// <param name="pathParameters"></param>
        /// <returns></returns>
        public static Parameters Deserialize(string pathParameters)
        {
            try
            {
                StaticObjects.Logger.Info("»»»» Deserialize Parameters");
                var jsonString = File.ReadAllText(pathParameters);
                return StaticObjects.DeserializeObject<Parameters>(jsonString);
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("»»»» Deserialize Parameters failure, returning default", ex);
                return new Parameters();
            }
        }


    }
}
