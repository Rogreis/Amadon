using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.InterchangeData
{
    /// <summary>
    /// Used to persist data from several app locations
    /// </summary>
    [Serializable]
    public class GenericData
    {
        public string HighlightedText { get; set; } = "";

        public List<string> SearchIndexEntries { get; set; } = new List<string>();

        public List<TOC_Entry> TrackEntries { get; set; } = new List<TOC_Entry>();

        public string LastTrackFileSaved { get; set; } = "";

        public string TextSearchForIndexTitles { get; set; } = "";

        public TubIndex TubIndex { get; set; }= new TubIndex();

        public List<TubIndexSubjects> IndexItemsFound { get; set; } = new List<TubIndexSubjects>();

        public bool browserSubjectsFound { get; set; } = false;

        public bool dropdownSubjectTitlesVisible { get; set; } = false;



    }
}
