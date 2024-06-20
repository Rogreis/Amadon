using AmadonStandardLib.UbClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmadonStandardLib.Classes
{
    public struct TitleData
    {
        public string ColumnSize;
        public string FirstLine;
        public string SecondLine;
    }

    public class PaperText
    {
        public TOC_Entry? Entry { get; set; } = null;

        public List<TitleData> Titles { get; set; } = new List<TitleData>();

        public List<string> Lines { get; set; } = new List<string>();
    }
}
