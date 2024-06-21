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

    public class ParagraphsLineData
    {
        public string? Identification = null;
        public TextShowOption? ShowOption = null;
        public Paragraph paragraphLeft = null;
        public string? MiddleText = null;
        public string? RightText = null;
    }

    public class PaperText
    {
        public TOC_Entry? Entry { get; set; } = null;

        public List<TitleData> Titles { get; set; } = new List<TitleData>();

        public List<ParagraphsLineData> Lines { get; set; } = new List<ParagraphsLineData>();
    }
}
