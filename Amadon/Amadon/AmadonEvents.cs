using AmadonStandardLib.InterchangeData;
using AmadonStandardLib.UbClasses;

namespace Amadon
{
    internal delegate void TopNavCommandDelegate(string controlName);
    internal delegate void TranslationsListInitializedDelegate();
    internal delegate void InitializationSuccesfullyDelegate();
    internal delegate void TranslationTocChangedDelegate();
    internal delegate void NewTocEntryDelegate(TOC_Entry entry);
    internal delegate void TranslationsToShowChangedDelegate();
    internal delegate void HighlightExpressionDelegate(string expression);
    internal delegate void NewSubjectIndexEntryDelegate(TOC_Entry entry);
    internal delegate void NewSearchEntryDelegate(TOC_Entry entry);
    internal delegate void NewTrackEntryDelegate(TOC_Entry entry);
    internal delegate void ShowTrackDelegate();
    internal delegate void ShowHelpPageDelegate(string helpPage);
    internal delegate void HelpContextDelegate();
    internal delegate void UpdateParagraphIdentDelegate(TOC_Entry entry);
    internal delegate void NewParagraphIdentDelegate(TOC_Entry entry);
    internal delegate void OpenEditNoteTextDelegate(UserNote note, bool isToDelete);
    internal delegate void EditNoteClosedDelegate(UserNote note, bool cancel);
    internal delegate void SearchNewTextDelegate(string text);
    internal delegate void SubjectSearchNewTextDelegate(string text);


    internal class AmandonComponentNames
    {
        public const string ControlSettings = "settings";
        public const string ControlSearch = "search";
        public const string ControlStudies = "studies";
        public const string ControlNotes = "notes";
        public const string ControlHelp = "help";
        public const string ControlTrack = "track";
        public const string ControlIndex = "index";
        public const string ControlToc = "toc";
    }

    internal class AmandonHelpPageNames
    {
        public const string TocHelp = "toc";
        public const string SearchHelp = "search";
        public const string StartingHelp = "help";
        public const string TrackHelp = "track";
        public const string NotesHelp = "notes";
        public const string IndexHelp = "index";
        public const string MenuBarHelp = "menubar";
        public const string SettingsHelp = "settings";
    }


    internal static class AmadonEvents
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Informs the system that a top navigation command has been issued.
        /// </summary>
        public static event TopNavCommandDelegate OnTopNavCommand;

        /// <summary>
        /// Informs the system that the list of translations has been initialized
        /// </summary>
        public static event TranslationsListInitializedDelegate OnTranslationsListInitialized;

        /// <summary>
        /// Fired when the initilization has finished succesfully
        /// </summary>
        public static event InitializationSuccesfullyDelegate OnInitializationSuccesfully;

        /// <summary>
        /// Fired when the translation to be used for TOC has changed
        /// </summary>
        public static event TranslationTocChangedDelegate OnTranslationTocChanged;

        /// <summary>
        /// Fired when the set of translation to have their text shown has changed
        /// </summary>
        public static event TranslationsToShowChangedDelegate OnTranslationsToShowChanged;

        /// <summary>
        /// Fired when a new table of contents item is selected
        /// </summary>
        public static event NewTocEntryDelegate OnNewTocEntry;

        /// <summary>
        /// Fired when a new paragraph is choosen in the search tool
        /// </summary>
        public static event NewSearchEntryDelegate OnNewSearchEntry;

        /// <summary>
        /// Fired when a new table of contents item is selected
        /// </summary>
        public static event NewSubjectIndexEntryDelegate OnNewSubjectIndexEntry;

        /// <summary>
        /// Used to ask for a hightlight action (same as a search) for an expression in the current text page
        /// </summary>
        public static event HighlightExpressionDelegate OnHighlightExpression;

        /// <summary>
        /// Used to force track component to update after a new entry added
        /// </summary>
        public static event ShowTrackDelegate? OnShowTrack;

        /// <summary>
        /// Used to inform text object asking to show a new paragraph
        /// </summary>
        public static event NewTrackEntryDelegate OnNewTrackEntry;

        /// <summary>
        /// Used to open specific help by the left page shown
        /// </summary>
        public static event ShowHelpPageDelegate OnShowHelpPage;

        /// <summary>
        /// Used to ask for a help page for the current left shown page
        /// </summary>
        public static event HelpContextDelegate OnHelpContext;

        /// <summary>
        /// Used to inform the top edit text for reference about a new paragraph
        /// </summary>
        public static event UpdateParagraphIdentDelegate OnUpdateParagraphIdent;

        /// <summary>
        /// Fired when user types a new paragraph identification in the top nav and hit enter
        /// </summary>
        public static event NewParagraphIdentDelegate OnNewParagraphIdent;

        /// <summary>
        /// Fired to open the big edit notes in the Boot Text componenet
        /// </summary>
        public static event OpenEditNoteTextDelegate OnOpenEditNoteText;


        public static event EditNoteClosedDelegate OnEditNoteClosed;

        /// <summary>
        /// Fired when a new text is selected in the book text to start a search
        /// </summary>
        public static event SearchNewTextDelegate OnSearchNewText;

        /// <summary>
        /// Fired when a new text is selected in the book text to start a search in the subjecy index
        /// </summary>
        public static event SubjectSearchNewTextDelegate OnSubjectSearchNewText;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        // ============================================================================================================= 
        // Functions to fire the above events

        public static void TopNavCommand(string controlName)
        {
            if (controlName == AmandonComponentNames.ControlHelp)
                ShowHelpPage(AmandonHelpPageNames.StartingHelp);
            else
                OnTopNavCommand?.Invoke(controlName);
        }

        public static void TranslationsListInitialized()
        {
            OnTranslationsListInitialized?.Invoke();
        }

        public static void InitializationSuccesfully()
        {
            OnInitializationSuccesfully?.Invoke();
        }

        public static void TranslationTocChanged()
        {
            OnTranslationTocChanged?.Invoke();
        }

        public static void NewTocEntry(TOC_Entry entry)
        {
            if (entry != null)
                OnNewTocEntry?.Invoke(entry);
        }

        public static void NewSubjectIndexEntry(TOC_Entry entry)
        {
            if (entry != null)
                OnNewSubjectIndexEntry?.Invoke(entry);
        }

        public static void NewSearchEntry(TOC_Entry entry)
        {
            if (entry != null)
                OnNewSearchEntry?.Invoke(entry);
        }
        // NewSearchEntryDelegate NewSearchEntry;

        public static void TranslationsToShowChanged()
        {
            OnTranslationsToShowChanged?.Invoke();
        }

        public static void HighlightExpression(string expression)
        {
            OnHighlightExpression?.Invoke(expression);
        }

        public static void ShowTrack()
        {
            OnShowTrack?.Invoke();
        }

        public static void NewTrackEntry(TOC_Entry entry)
        {
            OnNewTrackEntry?.Invoke(entry);
        }

        public static void ShowHelpPage(string newPage)
        {
            OnShowHelpPage?.Invoke(newPage);
        }

        public static void HelpContext()
        {
            OnHelpContext?.Invoke();
        }

        public static void BackToHelpHome()
        {
            ShowHelpPage(AmandonHelpPageNames.StartingHelp);
        }

        public static void UpdateParagraphIdent(TOC_Entry entry)
        {
            OnUpdateParagraphIdent?.Invoke(entry);
        }

        public static void NewParagraphIdent(TOC_Entry entry)
        {
            OnNewParagraphIdent?.Invoke(entry);
        }

        public static void OpenEditNoteText(UserNote note, bool isToDelete)
        {
            OnOpenEditNoteText?.Invoke(note, isToDelete);
        }

        public static void EditNoteClosed(UserNote note, bool cancel)
        {
            OnEditNoteClosed?.Invoke(note, cancel);
        }

        public static void SearchNewText(string text)
        {
            OnSearchNewText?.Invoke(text);
        }
        public static void SubjectSearchNewText(string text)
        {
            OnSubjectSearchNewText?.Invoke(text);
        }

    }
}
