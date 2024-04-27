
using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using System;
using System.Collections.Generic;

namespace AmadonStandardLib.UbClasses
{
    public class Book
    {
        protected DatabaseReader DataFiles = new DatabaseReader();

        public List<Translation>? Translations { get; set; } = null;

        public FormatTable? FormatTableObject { get; set; } = null;

        private const short EnglishId = 0;


        public Translation? EnglishTranslation
        {
            get
            {
                return GetTranslation(EnglishId);
            }
        }


        public Translation? LeftTranslation 
        { 
            get
            {
                return GetTranslation(StaticObjects.Parameters.LanguageIDLeftTranslation);
            }
        }

        public Translation? MiddleTranslation
        {
            get
            {
                return GetTranslation(StaticObjects.Parameters.LanguageIDMiddleTranslation);
            }
        }

        public Translation? RightTranslation
        {
            get
            {
                return GetTranslation(StaticObjects.Parameters.LanguageIDRightTranslation);
            }
        }



        /// <summary>
        /// Inicialize the list of available translations
        /// </summary>
        /// <returns></returns>
        protected bool InicializeTranslations()
        {
            try
            {
                if (Translations == null)
                {
                    Translations = DataFiles.GetTranslations();
                }
                return true;
            }
            catch (Exception ex)
            {
                string message = $"Could not initialize available translations. See log.";
                StaticObjects.Logger.Error(message, ex);
                LibraryEventsControl.FireFatalError(message);
                return false;
            }
        }


        /// <summary>
        /// Get a translation from the list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Translation? GetTranslation(short id)
        {
            Translation trans = Translations.Find(o => o.LanguageID == id);
            if (trans == null)
            {
                string message = $"Missing translation number {id}. May be you do not have the correct data to use this tool.";
                LibraryEventsControl.FireFatalError(message);
                return null;
            }
            return trans;
        }

        /// <summary>
        /// Initialize the format table used for editing translations
        /// </summary>
        public FormatTable? GetFormatTable()
        {
            try
            {
                if (FormatTableObject == null)
                {
                    FormatTableObject = new FormatTable(DataFiles.GetFormatTable());
                }
                return FormatTableObject;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error($"Missing format table. May be you do not have the correct data to use this tool.", ex);
                return null;
            }
        }

        public Translation? GetTocSearchTranslation()
        {
            switch (StaticObjects.Parameters.TranslationForTableOfContents)
            {
                case TranslatioForTocSearch.Left:
                    return StaticObjects.Book.LeftTranslation; 
                case TranslatioForTocSearch.Middle:
                    if (StaticObjects.Parameters.LanguageIDMiddleTranslation < 0)
                    {
                        if (StaticObjects.Parameters.LanguageIDRightTranslation < 0)
                            return StaticObjects.Book.LeftTranslation;
                        else
                            return StaticObjects.Book.RightTranslation;
                    }
                    return StaticObjects.Book.MiddleTranslation;
                case TranslatioForTocSearch.Right:
                    if (StaticObjects.Parameters.LanguageIDRightTranslation < 0)
                        return StaticObjects.Book.LeftTranslation;
                    else
                        return StaticObjects.Book.RightTranslation;
            }
            return StaticObjects.Book.LeftTranslation;
        }


    }
}
