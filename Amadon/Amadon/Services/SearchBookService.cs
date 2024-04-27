using AmadonStandardLib.Classes;
using AmadonStandardLib.InterchangeData;
using System.Text.Json;

namespace Amadon.Services
{
    public class SearchBookService
    {

        private static bool DoSearch(SearchData searchData)
        {
            LuceneBookSearch luceneBookSearch = new();
            // When an erro occurs, error message is set inside SearchData
            return luceneBookSearch.Execute(searchData);
         }

        public static Task<bool> Search(SearchData searchData)
        {
            return Task.FromResult(DoSearch(searchData));
        }


    }
}
