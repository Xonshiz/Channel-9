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
    public partial class AllShowPage : ContentPage
    {
        public AllShowPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (mainCollection.ItemsSource == null)
            {
                AllShows allShows = new AllShows();
                Shows shows = await allShows.getAllContent();
                mainCollection.ItemsSource = shows.result;
            }
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
    }
}