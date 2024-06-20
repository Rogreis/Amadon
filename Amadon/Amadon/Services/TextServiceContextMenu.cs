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

        /// <summary>
        /// Get the paragraphs list from a translations
        /// </summary>
        /// <param name="t"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static List<Paragraph> GetParagraphs(Translation t, TOC_Entry entry)
        {
            List<Paragraph> list = t?.Paper(entry.Paper).Paragraphs;
            list.ForEach(p => p.TranslationId = t.LanguageID);
            return list;
        }

        /// <summary>
        /// Format a table column "td" for a paragraph
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="par"></param>
        /// <param name="insertAnchor"></param>
        private static void Columntext(StringBuilder sb, Paragraph par, bool insertAnchor)
        {
            if (par != null)
            {
                string id = insertAnchor ? $" id =\"{par.AName}\"" : "";
                string divClass = "";
                if (par.TranslationId == StaticObjects.Book.GetTocSearchTranslation().LanguageID && par.Entry * StaticObjects.Parameters.Entry)
                {
                    divClass = "class=\"highlightedPar\"";
                }

                sb.AppendLine($"<td {id}><div {divClass} @onclick=\"@(() => ShowContextMenu(e, item))\">");
                sb.AppendLine(par.GetHtml(insertAnchor));
                sb.AppendLine("</div></td>");
            }
        }

        /// <summary>
        /// Get the formatted text when the paragraphs list is not null
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="list"></param>
        /// <param name="entry"></param>
        /// <param name="isEdit"></param>
        /// <param name="insertAnchor"></param>
        private static void GetText(StringBuilder sb, List<Paragraph> list, TOC_Entry entry, bool insertAnchor)
        {
            Paragraph par = list?.Find(p => p.Section == entry.Section && p.ParagraphNo == entry.ParagraphNo);
            Columntext(sb, par, insertAnchor);
        }

        /// <summary>
        /// Decision about which translations will be shown
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="leftParagraph"></param>
        /// <param name="rightParagraphs"></param>
        /// <param name="middleParagraphs"></param>
        private static void GetParagraphsLine(StringBuilder sb, Paragraph leftParagraph,
                                              List<Paragraph> rightParagraphs,
                                              List<Paragraph> middleParagraphs)
        {
            // Only first column has anchor
            Columntext(sb, leftParagraph, true);
            GetText(sb, middleParagraphs, leftParagraph.Entry, false);
            GetText(sb, rightParagraphs, leftParagraph.Entry, false);
        }

        /// <summary>
        /// Format a paper title
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static TitleData FormatTitle(Translation trans, TOC_Entry entry)
        {
            TitleData titleData = new TitleData();
            switch(ColumnSize)
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
        /// Service api
        /// </summary>
        /// <param name="href"></param>
        /// <returns>Json string for the object PaperText</returns>
        public static Task<PaperText> GetHtml()
        {
            PaperText paperTextFormatted = new PaperText();
            paperTextFormatted.Entry = StaticObjects.Parameters.Entry;

            // Get the paragraphs texts folowing what was required by user (StaticObjects.Parameters.TextShowOption)
            List<Paragraph>? leftParagraphs = null;
            List<Paragraph>? rightParagraphs = null;
            List<Paragraph>? middleParagraphs = null;

            // Left is always shown
            leftParagraphs = GetParagraphs(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry);

            // Calculate the text to show option
            switch (CalculateShowOption())
            {
                case TextShowOption.LeftOnly:
                    ColumnSize = columnSizeEnum.Width100;
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    break;
                case TextShowOption.LeftRight:
                    ColumnSize = columnSizeEnum.Width50;
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry);
                    break;
                case TextShowOption.LeftMiddle:
                    ColumnSize = columnSizeEnum.Width50;
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry));
                    middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry);
                    break;
                case TextShowOption.LeftMiddleRight:
                    ColumnSize = columnSizeEnum.Width33;
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.LeftTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry));
                    paperTextFormatted.Titles.Add(FormatTitle(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry));
                    rightParagraphs = GetParagraphs(StaticObjects.Book.RightTranslation, paperTextFormatted.Entry);
                    middleParagraphs = GetParagraphs(StaticObjects.Book.MiddleTranslation, paperTextFormatted.Entry);
                    break;
            }

            // Format line
            foreach (Paragraph p in leftParagraphs)
            {
                StringBuilder sb = new StringBuilder();
                GetParagraphsLine(sb, p, rightParagraphs, middleParagraphs);
                paperTextFormatted.Lines.Add(sb.ToString());
            }

            return Task.FromResult(paperTextFormatted);
        }
    }
}
