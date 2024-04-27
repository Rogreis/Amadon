using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;

namespace Amadon.Services
{
    internal class TrackService
    {


        static TrackService()
        {
            AmadonEvents.OnNewSubjectIndexEntry += OnNewTocEntry;
            AmadonEvents.OnNewTocEntry += OnNewTocEntry;
            AmadonEvents.OnNewSearchEntry += OnNewTocEntry;
        }

        public static void OnNewTocEntry(TOC_Entry entry)
        {
            AddEntry(entry);
            AmadonEvents.ShowTrack();
        }

        /// <summary>
        /// This function is called just to start this service to receive events
        /// </summary>
        public static void Dummy()
        { 
        }

        /// <summary>
        /// Add an entry to the tracking system
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static Task AddEntry(TOC_Entry entry)
        {
            if (PersistentData.GenericData.TrackEntries.Count == StaticObjects.Parameters.MaxExpressionsStored)
            {
                PersistentData.GenericData.TrackEntries.RemoveAt(PersistentData.GenericData.TrackEntries.Count - 1);
            }

            if (PersistentData.GenericData.TrackEntries.Count > 0 && PersistentData.GenericData.TrackEntries[0] * entry)
            {
                return Task.CompletedTask;
            }

            // Text is not saved and will be always filled with the same translation used for TOC and search
            entry.Text = "";
            PersistentData.GenericData.TrackEntries.Insert(0, entry);
            return Task.CompletedTask;
        }

        public static Task<List<string>> GetAllEntries() 
        {
            List<TOC_Entry> list= new List<TOC_Entry>(PersistentData.GenericData.TrackEntries);
            List<string> result= new List<string>();
            foreach (TOC_Entry entry in list) 
            {
                Paper paper = StaticObjects.Book.GetTocSearchTranslation().Paper(entry.Paper);
                Paragraph par = paper.GetParagraph(entry);
                if (par == null)
                {
                    entry.Text = "";
                    result.Add($"{entry} *** Error: not found");
                }
                else 
                {
                    result.Add(par.GetTrackHtml());
                }
            }
            return Task.FromResult(result);
        }
    }
}
