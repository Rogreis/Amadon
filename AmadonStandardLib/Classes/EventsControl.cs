using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using static AmadonStandardLib.Classes.LibraryEventsControl;

namespace AmadonStandardLib.Classes
{
    #region Delegates used to communicate between controls

    public delegate void RedrawTextDelegate();

    public delegate void FieldChangedDelegate();

    #endregion

    /// <summary>
    /// Used to send a fatal error message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    public delegate bool FatalErrorDelegate(string message, Exception ex);

    /// <summary>
    /// Used to send an error message
    /// </summary>
    /// <param name="message"></param>
    public delegate void ErrorDelegate(string message);

    /// <summary>
    /// Used to send a message to be shown in the main form
    /// </summary>
    /// <param name="Message"></param>
    public delegate void dlSendMessage(string Message);













    /// <summary>
    /// Used to fire a click on a new Table Of Contents item
    /// </summary>
    /// <param name="entry"></param>
    public delegate void dlTOCClicked(TOC_Entry entry);

    /// <summary>
    /// Used to indicate new track selected
    /// </summary>
    /// <param name="entry"></param>
    public delegate void dlTrackSelected(TOC_Entry entry);

    /// <summary>
    /// Used to fire a click on some seach result entry
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="Words"></param>
    public delegate void dlSearchClicked(TOC_Entry entry, List<string> Words);

    ///// <summary>
    ///// Used to fire a click on some seach result entry
    ///// </summary>
    ///// <param name="loc"></param>
    //internal delegate void dlDirectSearch(ParagraphSearchData data);

    /// <summary>
    /// Used to fire a click on index
    /// </summary>
    /// <param name="entry"></param>
    public delegate void dlIndexClicked(TOC_Entry entry);

    /// <summary>
    /// Used to ask for a new index entry
    /// </summary>
    /// <param name="indexEntry"></param>
    public delegate void dlOpenNewIndexEntry(string indexEntry);


    /// <summary>
    /// Used to infor about a change in the left column
    /// </summary>
    /// <param name="newWidth"></param>
    public delegate void dlGridSplitter(double newWidth);

    /// <summary>
    /// Used to communicate a change in he size of main window for objects that do not have a good resize
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public delegate void dlMainWindowSizeChanged(double width, double height);

    /// <summary>
    /// Fired after a new paper is shown
    /// </summary>
    public delegate void dlNewPaperShown();

    /// <summary>
    /// Used to ask to a text refresh in the main screen
    /// </summary>
    public delegate void dlRefreshText();

    /// <summary>
    /// Used to indicate that there are updates for translations and/application available 
    /// </summary>
    public delegate void dlUpdateAvailable();

    public delegate void dlTranslationsChanged();

    public delegate void dlRightTranslationChanged(Translation oRightTranslation);

    public delegate void dlBilingualChanged(bool ShowBilingual);


    //public delegate void dlFontChanged(ControlsAppearance appearance);

    //public delegate void dlAppearanceChanged(ControlsAppearance appearance);

    ///// <summary>
    ///// Used by SearchDataEntry to communicate a search for its parent control
    ///// </summary>
    ///// <param name="data"></param>
    //public delegate void dlShowSearchResults(SearchData data);

    /// <summary>
    /// Fired before to store an annotation
    /// </summary>
    /// <param name="xamlNotes"></param>
    public delegate void dsGetAnnotationData(ref string xamlNotes);

    /// <summary>
    /// Inform about changes in the annotations list
    /// </summary>
    public delegate void dlAnnotationsChanges(TOC_Entry entry);



    /// <summary>
    /// Implement the delegates and fire routines for the several events used
    /// </summary>
    public static class LibraryEventsControl
    {

        public static event RedrawTextDelegate? RedrawText = null;
        public static void FireRedrawText() => RedrawText?.Invoke();

        public static event FieldChangedDelegate? FieldChanged = null;
        public static void FireFieldChanged() => FieldChanged?.Invoke();


        public static event FatalErrorDelegate? FatalError = null;

        public static event ErrorDelegate? Error = null;

        public static event dlSendMessage? SendMessage = null;



        public static event dlSearchClicked? SearchClicked = null;

        //internal static event dlDirectSearch DirectSearch = null;

        public static event dlIndexClicked? IndexClicked = null;

        public static event dlOpenNewIndexEntry? OpenNewIndexEntry = null;

        public static event dlTOCClicked? TOCClicked = null;

        public static event dlTrackSelected? TrackSelected = null;


        public static event dlRefreshText? RefreshText = null;

        public static event dlUpdateAvailable? UpdateAvailable = null;

        public static event dlTranslationsChanged? TranslationsChanged = null;

        public static event dlBilingualChanged? BilingualChanged = null;

        //public static event dlFontChanged FontChanged = null;

        //public static event dlAppearanceChanged AppearanceChanged = null;

        public static event dlGridSplitter? GridSplitterChanged = null;

        public static event dlMainWindowSizeChanged? MainWindowSizeChanged = null;

        public static event dlNewPaperShown? NewPaperShown = null;

        internal static event dlAnnotationsChanges? AnnotationsChanges = null;

        internal static bool FireFatalError(string message, Exception? ex = null)
        {
            bool? ret = FatalError?.Invoke(message, ex);
            return ret != null ? ret.Value : false;
        }

        internal static void FireError(string message) => Error?.Invoke(message);

        internal static void FireSearchClicked(TOC_Entry entry, List<string> Words) => SearchClicked?.Invoke(entry, Words);

        public static void FireIndexClicked(TOC_Entry entry) => IndexClicked?.Invoke(entry);

        public static void FireOpenNewIndexEntry(string indexEntry) => OpenNewIndexEntry?.Invoke(indexEntry);

        public static void FireTOCClicked(TOC_Entry entry) => TOCClicked?.Invoke(entry);

        public static void FireTrackSelected(TOC_Entry entry) => TrackSelected?.Invoke(entry);

        public static void FireSendMessage(string location, Exception ex)
        {
            string message = location + ": ";
            Exception ex2 = ex;
            while (ex2 != null)
            {
                message += ex2.Message;
                ex2 = ex2.InnerException;
            }
            SendMessage?.Invoke(message);
        }

        //public static void FireShowExceptionMessage(string message, Exception ex, bool isFatal = false)
        //{
        //    ShowExceptionMessage?.Invoke(message, ex, isFatal);
        //}


        public static void FireGridSplitter(double newWidth) => GridSplitterChanged?.Invoke(newWidth);

        public static void FireMainWindowSizeChanged(double width, double height) => MainWindowSizeChanged?.Invoke(width, height);

        public static void FireSendMessage(string message) => SendMessage?.Invoke(message);

        public static void FireSendUserAndLogMessage(string message) 
        {
            FireSendMessage(message);
            StaticObjects.Logger.Info(message);
        }

        public static void FireSendUserAndLogMessage(string message, Exception ex)
        {
            FireSendMessage(message, ex);
            StaticObjects.Logger.Error(message, ex);
        }

        public static void FireRefreshText() => RefreshText?.Invoke();

        public static void FireUpdateAvailable() => UpdateAvailable?.Invoke();

        public static void FireTranslationsChanged() => TranslationsChanged?.Invoke();

        public static void FireBilingualChanged(bool ShowBilingual) => BilingualChanged?.Invoke(ShowBilingual);

        //public static void FireFontChanged() => FontChanged?.Invoke(((ParametersMAUI)StaticObjects.Parameters).Appearance);

        //public static void FireAppearanceChanged() => AppearanceChanged?.Invoke(((ParametersMAUI)StaticObjects.Parameters).Appearance);

        public static void FireNewPaperShown() => NewPaperShown?.Invoke();

        public static void FireAnnotationsChanges(TOC_Entry entry) => AnnotationsChanges?.Invoke(entry);


    }
}
