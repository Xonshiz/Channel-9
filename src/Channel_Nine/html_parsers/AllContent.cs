using Channel_Nine.models.AllContent;
using Channel_Nine.network;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Channel_Nine.html_parsers
{
    class AllContent
    {
        private AllContentResult _allContentResult;
        private int _currentPage = 1, _totalPages = 2619;
        private ObservableCollection<SingleResult> _singleResultCollection = new ObservableCollection<SingleResult>();
        //private GetWebPage getWebPage;

        public AllContent()
        {
            _allContentResult = new AllContentResult();
            //getWebPage = new GetWebPage();
        }
        public async Task<AllContentResult> getAllContent()
        {
            // This method will be publicly visible and will be the one consumed.
            string response = await GetWebPage.GetWebPageData(Common.ServiceUrls.allContentUrl); // Make a network call here to grab the content
            extractData(response);
            PopulateAllContentResult();
            return this._allContentResult;
        }

        private void extractData(string response)
        {
            // This will extract the Links, Thumbnails and Titles from the page.
            // Would return a List of that.
            var doc = new HtmlDocument();
            var _doc = new HtmlDocument();
            doc.LoadHtml(response);
            var articles = doc.DocumentNode.SelectNodes("//article[contains(@class,'abstract large')]");
            int i = 1;
            foreach (HtmlNode node in articles)
            {
                Console.WriteLine(node.Attributes);
                if (!string.IsNullOrEmpty(node.InnerHtml))
                {
                    _doc.LoadHtml(node.InnerHtml);
                    var episodeLanguage = node.Attributes["lang"].Value;
                    var episodeUrl = Common.ServiceUrls.baseUrl.Remove(Common.ServiceUrls.baseUrl.Length - 1, 1) + _doc.DocumentNode.SelectSingleNode("//a[@class='tile']").Attributes["href"].Value;
                    var showTitle = _doc.DocumentNode.SelectSingleNode("//header/div[@class='seriesTitle']").InnerText.Replace("&#160;", " ").Trim();
                    var episodeTitle = _doc.DocumentNode.SelectSingleNode("//header/h3").InnerText.Replace("&#160;", " ").Trim();
                    var episodeThumbnail = _doc.DocumentNode.SelectSingleNode("//a/img").Attributes["src"].Value;
                    var episodeRunTime = _doc.DocumentNode.SelectSingleNode("//a/time").InnerText.Replace("&#160;", " ").Trim();
                    PopulateEpisodeList(i.ToString(), episodeTitle.ToString(), episodeUrl.ToString(), episodeThumbnail.ToString(), episodeRunTime.ToString(), episodeLanguage.ToString(), showTitle.ToString());
                    i++; // Increment the episode ID.
                }
                else
                {
                    //We hit the empty ARTICLE Tag. Nothing to parse. Move along.
                    continue;
                }
            }
        }

        private void PopulateEpisodeList(string videoId, string videoTitle, string videoUrl, string videoThumbnail, string videoDuration, string videoLanguage, string showName)
        {
            this._singleResultCollection.Add(new SingleResult { videoDuration = videoDuration, videoID = videoId, videoImage = videoThumbnail, videoLanguage = videoLanguage, videoShow = showName, videoTitle = videoTitle, videoUrl = videoUrl });
        }

        private void PopulateAllContentResult()
        {
            this._allContentResult.currentPage = this._currentPage;
            this._allContentResult.lastFetchedTime = DateTime.UtcNow;
            this._allContentResult.totalPages = this._totalPages;
            this._allContentResult.result = this._singleResultCollection;
        }
    }
}
