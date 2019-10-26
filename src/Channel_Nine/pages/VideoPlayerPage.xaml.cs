using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Forms.VideoPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// Library Used For Video : https://github.com/pro777s/Xam.Forms.VideoPlayer
namespace Channel_Nine.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPlayerPage : ContentPage
    {
        private VideoSource _videoSource;

        public VideoPlayerPage(string streamUrl, string episodeTitle)
        {
            UriVideoSource uriVideoSurce = new UriVideoSource()
            {
                Uri = streamUrl
            };
            _videoSource = uriVideoSurce;
            this.Title = episodeTitle;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (videoPlayer.Source == null)
            {
                videoPlayer.Source = _videoSource;
                if (Device.RuntimePlatform == Device.Android)
                {
                    //NavigationPage.SetHasNavigationBar(this, false);
                    DependencyService.Get<IStatusBar>().HideStatusBar();
                }
            }
        }

        private async void VideoPlayer_PlayCompletion(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void VideoPlayer_PlayError(object sender, VideoPlayer.PlayErrorEventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected override void OnDisappearing()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                //NavigationPage.SetHasNavigationBar(this, true);
                DependencyService.Get<IStatusBar>().ShowStatusBar();
            }
            videoPlayer.Stop();
            base.OnDisappearing();
        }
    }
}