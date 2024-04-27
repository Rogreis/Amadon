using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AmadonStandardLib.UbClasses
{
    public class Paper
    {

        private string RepositoryFolder { get; set; } = "";
        private short paperEditNo = -1;
        private FormatTable? Format = null;

        [JsonIgnore]
        public short PaperNo
        {
            get
            {
                if (Paragraphs.Count > 0)
                {
                    return Paragraphs[0].Paper;
                }
                else
                {
                    return -1;
                }
            }
        }

        [JsonIgnore]
        public string Title
        {
            get
            {
                if (Paragraphs.Count > 0)
                {
                    return Paragraphs[0].Text;
                }
                else
                {
                    return "";
                }
            }
        }


        public List<Paragraph> Paragraphs { get; set; } = new List<Paragraph>();

        public TOC_Entry Entry
        {
            get
            {
                return new TOC_Entry(Paragraphs[0]);
            }
        }

        /// <summary>
        /// Constructor parameterless
        /// </summary>
        public Paper()
        {

        }

        /// <summary>
        /// Constructor from a json string 
        /// </summary>
        /// <param name="jsonString"></param>
        public Paper(string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };

            var fullPaper = JsonSerializer.Deserialize<FullPaper>(jsonString, options);
            Paragraphs.AddRange(fullPaper.Paragraphs);
            fullPaper = null;
        }


        /// <summary>
        /// Return an specific paragraph
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public Paragraph? GetParagraph(TOC_Entry entry)
        {
            return Paragraphs.Find(p => p.Paper == entry.Paper && p.Section == entry.Section && p.ParagraphNo == entry.ParagraphNo);
        }

        public override string ToString()
        {
            return $"Paper {PaperNo}";
        }
    }




}
