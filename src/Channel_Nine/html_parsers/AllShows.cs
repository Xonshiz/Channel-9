using Channel_Nine.models.Shows;
using Channel_Nine.network;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Channel_Nine.html_parsers
{
    class AllShows
    {
        private Shows _allShows;
        private int _currentPage = 1, _totalPages = 2619;
        private ObservableCollection<Show> _singleResultCollection = new ObservableCollection<Show>();
        //private GetWebPage getWebPage;

        public AllShows()
        {
            _allShows = new Shows();
            //getWebPage = new GetWebPage();
        }
        public async Task<Shows> getAllContent()
        {
            // This method will be publicly visible and will be the one consumed.
            string response = await GetWebPage.GetWebPageData(Common.ServiceUrls.allShowsUrl); // Make a network call here to grab the content
            extractData(response);
            PopulateAllContentResult();
            return this._allShows;
        }

        private void extractData(string response)
        {
            // This will extract the Links, Thumbnails and Titles from the page.
            // Would return a List of that.
            var doc = new HtmlDocument();
            var _doc = new HtmlDocument();
            doc.LoadHtml(response);
            var articles = doc.DocumentNode.SelectNodes("//article[contains(@class,'abstract xSmall noVideo')]");
            int i = 1;
            foreach (HtmlNode node in articles)
            {
                Console.WriteLine(node.Attributes);
                if (!string.IsNullOrEmpty(node.InnerHtml))
                {
                    _doc.LoadHtml(node.InnerHtml);
                    var showLanguage = node.Attributes["lang"].Value;
                    var showUrl = _doc.DocumentNode.SelectSingleNode("//a[@class='tile']").Attributes["href"].Value; // We remove Initial Back Slash to get ID of show that can be used further to get the ShowDetails.
                    var showTitle = _doc.DocumentNode.SelectSingleNode("//header/h3").InnerText.Replace("&#160;", " ").Trim();
                    var showThumbnail = _doc.DocumentNode.SelectSingleNode("//a/img").Attributes["src"].Value;
                    PopulateEpisodeList(i.ToString(), showLanguage.ToString(), showThumbnail.ToString(), showTitle.ToString(), showUrl.Split('/')[showUrl.Split('/').Length - 1].ToString());
                    i++; // Increment the episode ID.
                }
                else
                {
                    //We hit the empty ARTICLE Tag. Nothing to parse. Move along.
                    continue;
                }
            }
        }

        private void PopulateEpisodeList(string showId, string showLanguage, string showThumbnail, string showTitle, string showUrl)
        {
            this._singleResultCollection.Add(new Show { showID = showId, showLanguage = showLanguage, showThumbnail = showThumbnail, showTitle = showTitle, showUrl = showUrl, showNumber = Convert.ToInt32(showId) });
        }

        private void PopulateAllContentResult()
        {
            this._allShows.currentPage = this._currentPage;
            this._allShows.lastFetchedTime = DateTime.UtcNow;
            this._allShows.totalPages = this._totalPages;
            this._allShows.result = this._singleResultCollection;
        }
    }
}
