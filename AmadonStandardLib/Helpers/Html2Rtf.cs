using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace AmadonStandardLib.Helpers
{

    public enum RtfElementFontWeight
    {
        Thin = 100,
        Ultralight = 200,
        Light = 300,
        Regular = 400,
        Medium = 500,
        Semibold = 600,
        Bold = 700,
        Heavy = 800,
        Black = 900
    }

    public enum RtfElementFontAttributes
    {
        None = 0,
        Bold = 1,
        Italic = 2
    }

    public enum RtfElementTextDecorations
    {
        None = 0,
        Underline = 1,
        Strikethrough = 2
    }


    public class RtfElement
    {
        public RtfElement()
        { 
        }

        public string Text { get; set; } = "";
        public RtfElementFontWeight FontWeight { get; set; }= RtfElementFontWeight.Regular;
        public RtfElementFontAttributes FontAttributes { get; set; } = RtfElementFontAttributes.None;
        public RtfElementTextDecorations TextDecorations { get; set; }= RtfElementTextDecorations.None;

        public bool IsExternalLink { get; set; } = false;
        public string Href { get; set; } = "";

        //public TextTransform TextTransform { get; set; }
        //public Color BackgroundColor { get; set; }
        //public Style Style { get; set; }
        //public double CharacterSpacing { get; set; }
        //public virtual string UpdateFormsText(string source, TextTransform textTransform);
        //protected override void OnBindingContextChanged();
    }



    public class Html2Rtf
    {
        public static async Task<string> ProcessHtmlPage(string url)
        {
            string? htmlContent = await LoadHtmlContentAsync(url);
            if (htmlContent == null) 
            {
                return "Error";
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            return htmlContent;
        }

        private static async Task<string?> LoadHtmlContentAsync(string url)
        {
            using var client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string htmlContent = await response.Content.ReadAsStringAsync();
                return htmlContent;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }


        private static void ProcessNode(List<RtfElement> list, HtmlNode node)
        {
            foreach (var child in node.ChildNodes)
            {
                switch (child.Name.ToLower())
                {
                    case "a":
                        list.Add(CreateLinkSpan(child));
                        break;
                    case "b":
                    case "strong":
                        list.Add(CreateBoldSpan(child));
                        break;
                    case "i":
                    case "em":
                        list.Add(CreateItalicSpan(child));
                        break;
                    case "u":
                        list.Add(CreateUnderlineSpan(child));
                        break;
                    case "#text":
                        list.Add(new RtfElement { Text = child.InnerText });
                        break;
                    case "br":
                        list.Add(new RtfElement { Text = Environment.NewLine });
                        break;
                    // Add more cases here for other HTML elements
                    default:
                        ProcessNode(list, child);
                        break;
                }
            }
        }

        private static RtfElement CreateLinkSpan(HtmlNode node)
        {
            var span = new RtfElement
            {
                Text = node.InnerText,
                TextDecorations = RtfElementTextDecorations.Underline,
                IsExternalLink = node.Attributes["href"]?.Value is string href
            };

            if (span.IsExternalLink)
            {
                span.Href = node.Attributes["href"].Value.ToString();
            }

            return span;
        }

        private static RtfElement CreateBoldSpan(HtmlNode node)
        {
            return new RtfElement { Text = node.InnerText, FontWeight = RtfElementFontWeight.Bold };
        }

        private static RtfElement CreateItalicSpan(HtmlNode node)
        {
            return new RtfElement { Text = node.InnerText, FontAttributes = RtfElementFontAttributes.Italic };
        }

        private static RtfElement CreateUnderlineSpan(HtmlNode node)
        {
            return new RtfElement { Text = node.InnerText, TextDecorations = RtfElementTextDecorations.Underline };
        }
    }
}
