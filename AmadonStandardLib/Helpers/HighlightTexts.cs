using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AmadonStandardLib.Helpers
{
    public class HighlightTexts
    {
        public static string HighlightString(string html, string expression)
        {
            try
            {
                if (string.IsNullOrEmpty(expression))
                {
                    return html;
                }

                Regex regex = new Regex($"({expression})", RegexOptions.IgnoreCase);
                return regex.Replace(html, $"<span class=\"highlight\">{expression}</span>");
            }
            catch // Ignote errors
            {
                return html;
            }
        }

        public static string UnhighlightString(string html)
        {
            try
            {
                Regex regex = new Regex("<span class=\"highlight\">(.*?)</span>", RegexOptions.IgnoreCase);
                return regex.Replace(html, "$1");
            }
            catch // Ignote errors
            {
                return html;
            }
        }
    }
}
