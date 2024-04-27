using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace AmadonStandardLib.InterchangeData
{
    /// <summary>
    /// Stores a search result
    /// </summary>
    public class SearchResult : InterchangeDataBase
    {
        public TOC_Entry Entry { get; set; }

        public int OriginalPosition { get; set; } = -1;

        public string ID
        {
            get
            {
                return $"{Entry.Paper}:{Entry.Section}-{Entry.ParagraphNo}";
            }
        }


        public string Text
        {
            get
            {
                return Entry.Text;
            }
            set 
            {
                Entry.Text= value;
            }
        }


        public SearchResult(TOC_Entry entry)
        {
            Entry = entry;
        }

        public string LinkText
        {
            get
            {
                return $"{Entry.Paper};{Entry.Section};{Entry.ParagraphNo}";
            }
        }

        public override string ToString()
        {
            return Entry.ToShortString();
        }

    }
}
