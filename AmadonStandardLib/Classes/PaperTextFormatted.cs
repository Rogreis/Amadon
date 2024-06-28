using AmadonStandardLib.UbClasses;
using System.Collections.Generic;

namespace AmadonStandardLib.Classes
{
    public class PaperTextFormatted
    {
        public TOC_Entry? Entry { get; set; } = null;

        public List<string> Titles { get; set; } = new List<string>();

        public List<string> Lines { get; set; } = new List<string>();

    }
}
