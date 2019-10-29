using Channel_Nine.html_parsers;
using Channel_Nine.models.Shows;
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
    public partial class ShowDetailsPage : ContentPage
    {
        private string showId;
        private int _currentPage = 1;
        private ShowDetail showDetail;

        public ShowDetailsPage(string showId)
        {
            InitializeComponent();
            this.showDetail = new ShowDetail();
            this.showId = showId;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (this.episodeCollection.ItemsSource == null)
            {
                ShowDetails showDetails = new ShowDetails(this.showId, this._currentPage);
                this.showDetail = await showDetails.getAllContent();
                showTitleLabel.Text = this.showDetail.showTitle;
                // Would suggest that we change this from SHOWS class to List<Episode>. Because it's a list of EPISODES.
                // Or, it's technically like the shows class. So, we can do one thing, use the same class. Because when you click on Show.. you can redirect to SingleEpisde.
                // SingleEpisode has all the needed information.
                this.episodeCollection.ItemsSource = this.showDetail.showEpisodes.result;
            }
        }

        private void episodeCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.CurrentSelection[0] as Show;
            //DisplayAlert(selectedItem.showTitle, selectedItem.showUrl, "ok");
            //this.showId = URL OF THE SHOW
            //slectedItem.showUrl = ID/URL OF THE EPISODE.
            this.Navigation.PushAsync(new SingleEpisodePage(this.showId, selectedItem.showUrl.Split('/')[selectedItem.showUrl.Split('/').Length - 1]));
        }

        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }
    }
}