using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.Classes
{
    public class PaperTextFormatted
    {
        public TOC_Entry? Entry { get; set; } = null;

        public List<string> Titles { get; set; } = new List<string>();

        public List<string> Lines { get; set; } = new List<string>();

    }
}
