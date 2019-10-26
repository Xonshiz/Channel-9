using Channel_Nine.models.SingleEpisode;
using Channel_Nine.network;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Channel_Nine.html_parsers
{
    class VideoDetails
    {
        //private GetWebPage getWebPage;
        private SingleEpisode _singleEpisode;
        private videoData _videoData;
        private ObservableCollection<EpisodeComments> _episodeComments;
        private string _showId, _episodeId, _episodeTitle, _episodeDescription, _episodeRunTime;
        private List<string> _episodeTags = new List<string>();

        public VideoDetails(string showId, string episodeId)
        {
            //getWebPage = new GetWebPage();
            _singleEpisode = new SingleEpisode();
            _videoData = new videoData();
            _episodeComments = new ObservableCollection<EpisodeComments>();
            this._episodeId = episodeId;
            this._showId = showId;
        }
        public async Task<SingleEpisode> getAllContent()
        {
            // This method will be publicly visible and will be the one consumed.
            string response = await GetWebPage.GetWebPageData(Common.ServiceUrls.showUrl + this._showId + "/" + this._episodeId); // Make a network call here to grab the content
            extractData(response);
            PopulateEpisode();
            return this._singleEpisode;
        }

        private void extractData(string response)
        {
            // This will extract the Links, Thumbnails and Titles from the page.
            // Would return a List of that.
            var doc = new HtmlDocument();
            var _doc = new HtmlDocument();
            var _doc_ = new HtmlDocument();
            doc.LoadHtml(response);

            this._episodeTitle = doc.DocumentNode.SelectSingleNode("//meta[@property='og:title']").Attributes["content"].Value.ToString();
            this._episodeDescription = doc.DocumentNode.SelectSingleNode("//section[@class='ch9tab description']/div[@class='ch9tabContent']").InnerText.ToString();
            this._episodeRunTime = doc.DocumentNode.SelectSingleNode("//main[@class='playerContainer']/a/time[@class='caption']").InnerText.ToString();

            var episodeTags = doc.DocumentNode.SelectNodes("//div[@class='tags']");
            foreach (HtmlNode tag in episodeTags)
            {
                _doc.LoadHtml(tag.InnerHtml);
                this._episodeTags.Add(_doc.DocumentNode.SelectSingleNode("//a").InnerText.ToString().Trim());
            }

            this._videoData.videoID = this._episodeId;
            var episodeStreams = doc.DocumentNode.SelectNodes("//section[@class='ch9tab download']/div[@class='ch9tabContent']/div[@class='download']/div/ul");
            if (episodeStreams != null)
            {
                this._videoData.audioStreams = new Dictionary<Common.InternalTypes.audioQuality, string>() { };
                this._videoData.videoStreams = new Dictionary<Common.InternalTypes.videoQuality, string>() { };
                this._videoData.subtitleStreams = new Dictionary<string, string>() { };
                _doc.LoadHtml(episodeStreams[0].InnerHtml);
                foreach (HtmlNode node in _doc.DocumentNode.ChildNodes)
                {
                    if (!string.IsNullOrEmpty(node.InnerHtml.Trim()))
                    {
                        _doc_.LoadHtml(node.InnerHtml.Trim());
                        var streamQuality = _doc_.DocumentNode.SelectSingleNode("//a").InnerText.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        var streamLink = _doc_.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value.ToString().Trim();

                        if (streamLink != null)
                        {
                            if (streamLink.Contains(".mp4"))
                            {
                                if (streamQuality.Contains("Low Quality MP4"))
                                    this._videoData.videoStreams.Add(Common.InternalTypes.videoQuality.Low, streamLink);
                                else if (streamQuality.Contains("Mid Quality MP4"))
                                    this._videoData.videoStreams.Add(Common.InternalTypes.videoQuality.Medium, streamLink);
                                else if (streamQuality.Contains("High Quality MP4"))
                                    this._videoData.videoStreams.Add(Common.InternalTypes.videoQuality.High, streamLink);
                            }
                            else
                            {
                                if (!this._videoData.audioStreams.ContainsKey(Common.InternalTypes.audioQuality.MP3))
                                {
                                    this._videoData.audioStreams.Add(Common.InternalTypes.audioQuality.MP3, streamLink);
                                }
                            }
                        }
                    }
                }
                if (episodeStreams.Count > 0)
                {
                    _doc.LoadHtml(episodeStreams[1].InnerHtml);
                    foreach (HtmlNode node in _doc.DocumentNode.ChildNodes)
                    {
                        if (!string.IsNullOrEmpty(node.InnerHtml.Trim()))
                        {
                            _doc_.LoadHtml(node.InnerHtml.Trim());
                            var subtitleLanguage = _doc_.DocumentNode.SelectSingleNode("//a").InnerText.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                            var subtitleLink = _doc_.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value.ToString().Trim();

                            if (!this._videoData.subtitleStreams.ContainsKey(subtitleLanguage))
                            {
                                this._videoData.subtitleStreams.Add(subtitleLanguage, subtitleLink);
                            }
                        }
                    }
                }
            }


            var episodeComents = doc.DocumentNode.SelectNodes("//ul[@class='discussion']/li");
            if (episodeComents != null)
            {
                foreach (HtmlNode episodeComent in episodeComents)
                {
                    _doc.LoadHtml(episodeComent.InnerHtml);
                    var commentId = _doc.DocumentNode.SelectSingleNode("//div[@class='details withAuthor']").Attributes["id"].Value.ToString().Trim();
                    var commentAuthorName = _doc.DocumentNode.SelectSingleNode("//figure/figcaption").InnerText.ToString().Replace("&#160;", "").Trim();
                    var commentText = _doc.DocumentNode.SelectSingleNode("//div[@class='details withAuthor']").InnerText.ToString().Replace("&#160;", "").Trim();
                    var commentTime = _doc.DocumentNode.SelectSingleNode("//footer/div[@class='modified']/time/a").InnerText.ToString().Replace("&#160;", "").Trim();
                    this._episodeComments.Add(new EpisodeComments { commentAuthor = commentAuthorName, commentData = commentText, commentID = commentId, commentTime = commentTime });
                }
            }

        }

        private void PopulateEpisode()
        {
            this._singleEpisode.episodeDescription = this._episodeDescription;
            this._singleEpisode.episodeID = this._episodeId;
            this._singleEpisode.episodeTags = this._episodeTags;
            this._singleEpisode.episodeTitle = this._episodeTitle;
            this._singleEpisode.epsiodeStreams = this._videoData;
            this._singleEpisode.episodeComments = this._episodeComments;
            this._singleEpisode.episodeRunTime = this._episodeRunTime;
        }

    }
}
