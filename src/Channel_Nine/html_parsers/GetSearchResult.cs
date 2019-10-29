using Channel_Nine.network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Channel_Nine.models.SearchResults
{
    class GetSearchResult
    {
        private string _searchTerm;
        private SearchResult _searchResult;
        public GetSearchResult(string searchTerm)
        {
            _searchResult = new SearchResult();
        }

        public async Task<SearchResult> getAllContent()
        {
            // This method will be publicly visible and will be the one consumed.
            //string parameters = "?page=" + this._currentPage.ToString();
            string response = await GetWebPage.GetWebPageData(Common.ServiceUrls.searchUrl + this._searchTerm); // Make a network call here to grab the content
            extractData(response);
            return this._searchResult;
        }

        private void extractData(string response)
        {
            // This will extract the Links, Thumbnails and Titles from the page.
            // Would return a List of that.
            this._searchResult = JsonConvert.DeserializeObject<SearchResult>(response);
        }
    }
}
