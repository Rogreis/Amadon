using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AmadonStandardLib.UbClasses
{
    /// <summary>
    /// Define a table of contents entry for TUB
    /// </summary>
    public class TUB_TOC_Entry
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        public short PaperNo { get; set; } = -1;

        public short SectionNo { get; set; } = -1;

        public short ParagraphNo { get; set; } = -1;

        [JsonPropertyName("expanded")]
        public bool Expanded { get; set; }

        [JsonPropertyName("nodes")]
        public List<TUB_TOC_Entry> Nodes { get; set; } = new List<TUB_TOC_Entry>();

        public override string ToString()
        {
            return Text;
        }
    }
}
