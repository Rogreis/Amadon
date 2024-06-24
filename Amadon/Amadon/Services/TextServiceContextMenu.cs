using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.UbClasses;
using Blazorise;
using System.Text;
using Paragraph = AmadonStandardLib.UbClasses.Paragraph;

namespace Amadon.Services
{
    internal class TextServiceContextMenu
    {
        enum columnSizeEnum
        {
            Width25,
            Width33,
            Width50,
            Width100
        }
        private static columnSizeEnum ColumnSize = columnSizeEnum.Width50;


        #region Title Data
        /// <summary>
        /// Format a paper title
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static TitleData GetTitleData(Translation trans, TOC_Entry entry)
        {
            TitleData titleData = new TitleData();
            switch (ColumnSize)
            {
                case columnSizeEnum.Width25:
                    titleData.ColumnSize = "width-25";
                    break;
                case columnSizeEnum.Width33:
                    titleData.ColumnSize = "width-33";
                    break;
                case columnSizeEnum.Width50:
                    titleData.ColumnSize = "width-50";
                    break;
                case columnSizeEnum.Width100:
                    titleData.ColumnSize = "width-100";
                    break;

            }
            titleData.FirstLine = trans.Description;
            titleData.SecondLine = $"{trans.PaperTranslation} {entry.Paper}";
            return titleData;
        }

        #endregion


        ///// <summary>
        ///// Format a table column "td" for a paragraph
        ///// </summary>
        ///// <param name="sb"></param>
        ///// <param name="par"></param>
        ///// <param name="insertAnchor"></param>
        //private static void Columntext(StringBuilder sb, Paragraph par, bool insertAnchor)
        //{
        //    if (par != null)
        //    {
        //        string id = insertAnchor ? $" id =\"{par.AName}\"" : "";
        //        string divClass = "";
        //        if (par.TranslationId == StaticObjects.Book.GetTocSearchTranslation().LanguageID && par.Entry * StaticObjects.Parameters.Entry)
        //        {
        //            divClass = "class=\"highlightedPar\"";
        //        }

        //        sb.AppendLine($"<td {id}><div {divClass} @onclick=\"@(() => ShowContextMenu(e, item))\">");
        //        sb.AppendLine(par.GetHtml(insertAnchor));
        //        sb.AppendLine("</div></td>");
        //    }
        //}

        ///// <summary>
        ///// Get the formatted text when the paragraphs list is not null
        ///// </summary>
        ///// <param name="sb"></param>
        ///// <param name="list"></param>
        ///// <param name="entry"></param>
        ///// <param name="isEdit"></param>
        ///// <param name="insertAnchor"></param>
        //private static void GetText(StringBuilder sb, List<Paragraph> list, TOC_Entry entry, bool insertAnchor)
        //{
        //    Paragraph par = list?.Find(p => p.Section == entry.Section && p.ParagraphNo == entry.ParagraphNo);
        //    Columntext(sb, par, insertAnchor);
        //}

        ///// <summary>
        ///// Decision about which translations will be shown
        ///// </summary>
        ///// <param name="sb"></param>
        ///// <param name="leftParagraph"></param>
        ///// <param name="rightParagraphs"></param>
        ///// <param name="middleParagraphs"></param>
        //private static void GetParagraphsLine(StringBuilder sb, Paragraph leftParagraph,
        //                                      List<Paragraph> rightParagraphs,
        //                                      List<Paragraph> middleParagraphs)
        //{
        //    // Only first column has anchor
        //    Columntext(sb, leftParagraph, true);
        //    GetText(sb, middleParagraphs, leftParagraph.Entry, false);
        //    GetText(sb, rightParagraphs, leftParagraph.Entry, false);
        //}


        private static TextShowOption CalculateShowOption()
        {
            StaticObjects.Parameters.ShowRight = StaticObjects.Parameters.ShowRight && StaticObjects.Parameters.TranslationsToShowId.Count > 0 && StaticObjects.Parameters.LanguageIDRightTranslation >= 0;
            StaticObjects.Parameters.ShowMiddle = StaticObjects.Parameters.ShowMiddle && StaticObjects.Parameters.TranslationsToShowId.Count > 1 && StaticObjects.Parameters.LanguageIDMiddleTranslation >= 0;

            if (StaticObjects.Parameters.ShowRight && StaticObjects.Parameters.ShowMiddle)
            {
                return TextShowOption.LeftMiddleRight;
            }
            if (!StaticObjects.Parameters.ShowRight && StaticObjects.Parameters.ShowMiddle)
            {
                return TextShowOption.LeftMiddle;
            }
            if (StaticObjects.Parameters.ShowRight && !StaticObjects.Parameters.ShowMiddle)
            {
                return TextShowOption.LeftRight;
            }
            return TextShowOption.LeftOnly;
        }


        /// <summary>
        /// Create a paragraph object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static Paragraph CreateParagraph(string text)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Text = text;
            return paragraph;
        }

        /// <summary>
        /// Get a paragraph from translation
        /// </summary>
        /// <param name="t"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static Paragraph GetParagraph(Translation? t, TOC_Entry entry)
        {
            if (t == null) return CreateParagraph("Translation not found");
            Paragraph? par = t.Paper(entry.Paper).Paragraphs.Find(p => p.Paper == entry.Paper && p.Section == entry.Section && p.ParagraphNo == entry.ParagraphNo);
            if (par == null) return CreateParagraph("Paragraph not found");
            return par;
        }



        /// <summary>
        /// Service api
        /// </summary>
        /// <param name="href"></param>
        /// <returns>Json string for the object PaperText</returns>
        public static Task<PaperText> GetHtml()
        {
            PaperText paperText = new PaperText();
            paperText.Entry = StaticObjects.Parameters.Entry;
            paperText.Lines = [];


            // Calculate the text to show option
            switch (CalculateShowOption())
            {
                case TextShowOption.LeftOnly:
                    ColumnSize = columnSizeEnum.Width100;
                    paperText.Titles.Add(GetTitleData(StaticObjects.Book.LeftTranslation, paperText.Entry));
                    break;

                case TextShowOption.LeftRight:
                    ColumnSize = columnSizeEnum.Width50;
                    paperText.Titles.Add(GetTitleData(StaticObjects.Book.LeftTranslation, paperText.Entry));
                    paperText.Titles.Add(GetTitleData(StaticObjects.Book.RightTranslation, paperText.Entry));
                    break;

                case TextShowOption.LeftMiddle:
                    ColumnSize = columnSizeEnum.Width50;
                    paperText.Titles.Add(GetTitleData(StaticObjects.Book.LeftTranslation, paperText.Entry));
                    paperText.Titles.Add(GetTitleData(StaticObjects.Book.MiddleTranslation, paperText.Entry));
                    break;

                case TextShowOption.LeftMiddleRight:
                    ColumnSize = columnSizeEnum.Width33;
                    paperText.Titles.Add(GetTitleData(StaticObjects.Book.LeftTranslation, paperText.Entry));
                    paperText.Titles.Add(GetTitleData(StaticObjects.Book.MiddleTranslation, paperText.Entry));
                    paperText.Titles.Add(GetTitleData(StaticObjects.Book.RightTranslation, paperText.Entry));
                    break;
            }


            // Get all text according user option
            foreach (Paragraph p in StaticObjects.Book.LeftTranslation?.Paper(StaticObjects.Parameters.Entry.Paper).Paragraphs)
            {
                ParagraphsLineData paragraphsLineData = new ParagraphsLineData();
                paragraphsLineData.Identification = p.Identification;
                paragraphsLineData.paragraphLeft = p.DeepCopy();
                paragraphsLineData.ShowOption = CalculateShowOption();

                // Calculate the text to show option
                switch (paragraphsLineData.ShowOption)
                {
                    case TextShowOption.LeftOnly:
                        ColumnSize = columnSizeEnum.Width100;
                        break;

                    case TextShowOption.LeftRight:
                        ColumnSize = columnSizeEnum.Width50;
                        paragraphsLineData.RightText = GetParagraph(StaticObjects.Book.RightTranslation, p.Entry).GetHtml();
                        break;

                    case TextShowOption.LeftMiddle:
                        ColumnSize = columnSizeEnum.Width50;
                        paragraphsLineData.MiddleText = GetParagraph(StaticObjects.Book.MiddleTranslation, p.Entry).GetHtml();
                        break;

                    case TextShowOption.LeftMiddleRight:
                        ColumnSize = columnSizeEnum.Width33;
                        paragraphsLineData.MiddleText = GetParagraph(StaticObjects.Book.MiddleTranslation, p.Entry).GetHtml();
                        paragraphsLineData.RightText = GetParagraph(StaticObjects.Book.RightTranslation, p.Entry).GetHtml();
                        break;
                }
                paperText.Lines.Add(paragraphsLineData);
            }

            return Task.FromResult(paperText);
        }
    }
}
