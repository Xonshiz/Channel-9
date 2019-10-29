using Channel_Nine.models;
using Channel_Nine.models.AllContent;
using Channel_Nine.models.Shows;
using Channel_Nine.network;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

/*
 * Using SHOW() because the episode list that comes up is just like the list of SHOWS that come in ALL SHOWS.
 * RE-USE, MUTHAFUCKA!
 */

namespace Channel_Nine.html_parsers
{
    class ShowDetails
    {
        //private GetWebPage getWebPage;
        private ShowDetail _show;
        private Shows _allShows;
        private string _showId, _showDescription, _showTitle;
        private List<Authors> _showAuthors;
        private int _currentPage = 1, _totalPages = 1;
        private ObservableCollection<Show> _singleResultCollection = new ObservableCollection<Show>();

        public ShowDetails(string showId, int current_page = 1)
        {
            //getWebPage = new GetWebPage();
            _show = new ShowDetail();
            _allShows = new Shows();
            _showAuthors = new List<Authors>();
            this._showId = showId;
            this._currentPage = current_page;
        }

        public async Task<ShowDetail> getAllContent()
        {
            string parameters = "?page=" + this._currentPage.ToString();
            // This method will be publicly visible and will be the one consumed.
            string response = await GetWebPage.GetWebPageData(Common.ServiceUrls.showUrl + this._showId + parameters); // Make a network call here to grab the content
            extractData(response);
            PopulateAllShow();
            PopulateShowDetail();
            return this._show;
        }

        private void extractData(string response)
        {
            // This will extract the Links, Thumbnails and Titles from the page.
            // Would return a List of that.
            var doc = new HtmlDocument();
            var _doc = new HtmlDocument();
            doc.LoadHtml(response);
            try
            {
                this._currentPage = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//span[@class='selected']").InnerText.ToString().Trim());
            }
            catch (Exception)
            {
                this._currentPage = 1;
            }
            try
            {
                this._totalPages = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//li[@class='next']/a").InnerText.ToString().Trim());
            }
            catch (Exception)
            {
                // 1 by default should be the last page if anything fails. We don't want index out of bound errors.
                this._totalPages = 1;
            }
            var authors = doc.DocumentNode.SelectNodes("//figure[@role='presentation']");

            if (authors != null)
            {
                foreach (HtmlNode author in authors)
                {
                    _doc.LoadHtml(author.InnerHtml);
                    var authorName = _doc.DocumentNode.SelectSingleNode("//a").Attributes["aria-label"].Value.ToString();
                    var authorImage = _doc.DocumentNode.SelectSingleNode("//a/img").Attributes["src"].Value.ToString();
                    var authorProfileLink = Common.ServiceUrls.baseUrl.Remove(Common.ServiceUrls.baseUrl.Length - 1, 1) + _doc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value.ToString();
                    this._showAuthors.Add(new Authors { authorName = authorName, authorImage = authorImage, authorProfileLink = authorProfileLink });
                }
            }

            try
            {
                var latest_episode = doc.DocumentNode.SelectSingleNode("//article[@class='abstract xLarge latest']");
                var _episodeTitle = latest_episode.SelectSingleNode("//a[@class='tile']/img").Attributes["alt"].Value.Replace("&#160;", " ").Trim();
                var _episodeLanguage = latest_episode.Attributes["lang"].Value;
                var _episodeUrl = Common.ServiceUrls.baseUrl.Remove(Common.ServiceUrls.baseUrl.Length - 1, 1) + latest_episode.SelectSingleNode("//a[@class='tile']").Attributes["href"].Value;
                var _episodeThumbnail = latest_episode.SelectSingleNode("//a/img").Attributes["src"].Value;
                PopulateEpisodeList("1", _episodeLanguage.ToString(), _episodeThumbnail.ToString(), _episodeTitle.ToString(), _episodeUrl.ToString());
            }
            catch (Exception)
            {
                Console.WriteLine("No Latest Show");
            }

            try
            {
                this._showDescription = Convert.ToString(doc.DocumentNode.SelectSingleNode("//div[@class='fullText']").InnerText);
            }
            catch (Exception)
            {
                this._showDescription = Convert.ToString(doc.DocumentNode.SelectSingleNode("//ul[@class='metadata']/li[@class='description']").InnerText);
            }
            this._showTitle = Convert.ToString(doc.DocumentNode.SelectSingleNode("//title").InnerText).Replace("&#124; Channel 9", "").Trim();

            var articles = doc.DocumentNode.SelectNodes("//article[contains(@class,'abstract small')]");
            /*
             * We will start i from 2 becasue the latest episode is in another DIV, which needs to be extracted separately.
             * So, value of i for that episode will be 1 and for rest, it'll be 2 and so forth.
             */
            if (articles != null)
            {
                int i = 2;
                foreach (HtmlNode node in articles)
                {
                    Console.WriteLine(node.Attributes);
                    if (!string.IsNullOrEmpty(node.InnerHtml))
                    {
                        _doc.LoadHtml(node.InnerHtml);
                        var episodeLanguage = node.Attributes["lang"].Value;
                        var episodeUrl = Common.ServiceUrls.baseUrl.Remove(Common.ServiceUrls.baseUrl.Length - 1, 1) + _doc.DocumentNode.SelectSingleNode("//a[@class='tile']").Attributes["href"].Value;
                        var episodeTitle = _doc.DocumentNode.SelectSingleNode("//header/h3").InnerText.Replace("&#160;", " ").Trim();
                        var episodeThumbnail = _doc.DocumentNode.SelectSingleNode("//a/img").Attributes["src"].Value;
                        PopulateEpisodeList(i.ToString(), episodeLanguage.ToString(), episodeThumbnail.ToString(), episodeTitle.ToString(), episodeUrl.ToString());
                        i++; // Increment the episode ID.
                    }
                    else
                    {
                        //We hit the empty ARTICLE Tag. Nothing to parse. Move along.
                        continue;
                    }
                }
            }
        }

        private void PopulateEpisodeList(string showId, string showLanguage, string showThumbnail, string showTitle, string showUrl)
        {
            this._singleResultCollection.Add(new Show { showID = showId, showLanguage = showLanguage, showThumbnail = showThumbnail, showTitle = showTitle, showUrl = showUrl, showNumber = Convert.ToInt32(showId) });
        }

        private void PopulateAllShow()
        {
            this._allShows.currentPage = this._currentPage;
            this._allShows.lastFetchedTime = DateTime.UtcNow;
            this._allShows.totalPages = this._totalPages;
            this._allShows.result = this._singleResultCollection;
        }

        private void PopulateShowDetail()
        {
            this._show.showId = this._showId;
            this._show.showTitle = this._showTitle;
            this._show.showEpisodes = this._allShows;
            this._show.showDescription = this._showDescription;
            this._show.showAuthors = this._showAuthors;
        }
    }
}
