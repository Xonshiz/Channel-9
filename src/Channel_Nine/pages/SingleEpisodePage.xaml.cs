using Channel_Nine.html_parsers;
using Channel_Nine.models.SingleEpisode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Channel_Nine.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SingleEpisodePage : ContentPage
    {
        private string _showId, _episodeId;
        private SingleEpisode singleEpisode;

        public SingleEpisodePage(string showId, string episodeId)
        {
            InitializeComponent();
            this._showId = showId;
            this._episodeId = episodeId;
            singleEpisode = new SingleEpisode();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            // This is because we don't want to load the data again and again.
            if (string.IsNullOrEmpty(episodeTitle.Text))
            {
                VideoDetails showDetails = new VideoDetails(this._showId, this._episodeId);
                this.singleEpisode = await showDetails.getAllContent();
                episodeTitle.Text = this.singleEpisode.episodeTitle;
            }
        }

        private async void playVideoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VideoPlayerPage(this.singleEpisode.epsiodeStreams.videoStreams[Common.InternalTypes.videoQuality.Low], this.singleEpisode.episodeTitle), true);
        }

    }
}