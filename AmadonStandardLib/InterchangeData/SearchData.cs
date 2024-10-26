using AmadonStandardLib.UbClasses;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;

namespace AmadonStandardLib.InterchangeData
{
    /// <summary>
    /// Used to send/receive data to/from the book search engine
    /// </summary>
    public class SearchData : InterchangeDataBase
    {
        public bool OrderedByParagraphs { get; set; } = false;

        public Translation? Translation { get; set; } = null;

        public string IndexPathRoot { get; set; } = "";

        public bool Part1Included { get; set; } = true;

        public bool Part2Included { get; set; } = true;

        public bool Part3Included { get; set; } = true;

        public bool Part4Included { get; set; } = true;

        public bool CurrentPaperOnly { get; set; } = false;

        public string QueryString { get; set; } = "";

        public short CurrentPaper { get; set; } = -1;

        public string currentPage { get; set; } = "1";
        public string totalPages { get; set; } = "0";

        public int maxPaginationPages { get; set; } = 25; // Max number of pages
        public int maxPaginationItems { get; set; } = 5;  // Max number of pages shown in the pagination
        public int paginationFirst { get; set; } = 1;     // First page number in pagination
        public int paginationLast { get; set; } = 5;      // Last page number in pagination

        public int maxItemsPerPage { get; set; } = 20;    // Max number of items shown per page
        public int firstItemToShow { get; set; } = 1;     // Number of the first search result to show
        public int lastItemToShwow { get; set; } = 20;    // Number of the last search result to show


        public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();

        private string PartsIncluded
        {
            get
            {
                if (CurrentPaperOnly)
                {
                    return "Current Paper only";
                }
                string parts = "";
                if (Part1Included) { parts += "I"; }
                if (Part2Included) { parts += " II"; }
                if (Part3Included) { parts += " III"; }
                if (Part4Included) { parts += " IV"; }
                if (string.IsNullOrWhiteSpace(parts)) return "No part choosen to be searched.";
                return "Searched in: " + parts.Trim();
            }
        }

        public string SearchResultsMessage
        {
            get
            {
                switch (SearchResults.Count)
                {
                    case 0:
                        return $"Nothing found. {PartsIncluded}";
                    case 1:
                        return $"1 result found. {PartsIncluded}";
                    default:
                        return $"{SearchResults.Count} results found. {PartsIncluded}";
                }
            }
        }

        public List<string> Words { get; set; } = new List<string>();

        public bool paginationVisible { get; set; } = false;
        public bool hasDataToShow { get; set; } = false;

        public bool IsPaperIncluded(int PaperNo) =>
                    Part1Included && PaperNo < 32 ||
                    Part2Included && PaperNo >= 32 && PaperNo <= 56 ||
                    Part3Included && PaperNo >= 57 && PaperNo <= 119 ||
                    Part4Included && PaperNo >= 120;

        public void Clear()
        {
            SearchResults.Clear();
            Words.Clear();
            OrderedByParagraphs = false;
            paginationVisible = false;
            hasDataToShow = false;
        }


        private string PrepareWordToHighligth(string word)
        {
            return word.Trim().Replace("~", "").Replace("^", "").Replace("*", "");
        }

        /// <summary>
        /// Create a list of words to highligh in the search
        /// </summary>
        /// <param name="query"></param>
        public void ExtractTerms(Query query)
        {
            var terms = new HashSet<Term>();
            try
            {
                Words.Clear();

                query.ExtractTerms(terms);
                foreach (Term t in terms)
                {
                    Words.Add(PrepareWordToHighligth(t.Text));
                }
            }
            catch
            {
                // Try to get words when ExtractTerms is not implemented
                string queryString = query.ToString().Replace("?", "").Replace("*", "").Replace("~", "").Replace("(", "").Replace(")", "");
                string[] parts = queryString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Words.AddRange(parts);
            }
        }

        /// <summary>
        /// Sort the results by paragraphs or original order (ranking done by Lucene)
        /// </summary>
        public void SortResults()
        {
            if (SearchResults.Count == 0)
            {
                return;
            }
            if (!OrderedByParagraphs)
            {
                SearchResults.Sort((a, b) => a.Entry.CompareTo(b.Entry));
                OrderedByParagraphs = true;
            }
            else
            {
                SearchResults.Sort((a, b) => a.OriginalPosition.CompareTo(b.OriginalPosition));
                OrderedByParagraphs = false;
            }
        }

    }
}
