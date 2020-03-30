using Channel_Nine.html_parsers;
using Channel_Nine.models.Shows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Channel_Nine.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllShowPage : ContentPage
    {
        private int _currentPage = 0;
        private AllShows allShows;
        private Shows shows;
        private ObservableCollection<Show> resultList = new ObservableCollection<Show>() { };
        public AllShowPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (mainCollection.ItemsSource == null)
            {
                allShows = new AllShows();
                getMoreResults();
                setListViewBinding();
            }
        }

        private void setListViewBinding()
        {
            mainCollection.ItemsSource = this.resultList;
        }

        private void mainCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.CurrentSelection[0] as Show;
            //DisplayAlert(selectedItem.showTitle, selectedItem.showUrl, "ok");
            this.Navigation.PushAsync(new ShowDetailsPage(selectedItem.showUrl));
        }

        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }

        async void getMoreResults(object sender = null, EventArgs e = null)
        {
            shows = await allShows.getAllContent(this._currentPage + 1);
            foreach (Show show in shows.result)
            {
                resultList.Add(show);
            }
        }
    }
}