using Channel_Nine.html_parsers;
using Channel_Nine.models.AllContent;
using Channel_Nine.models.Shows;
using Channel_Nine.models.SingleEpisode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Channel_Nine
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            myLable.Text = "SUP?!";
            //AllContent allContent = new AllContent();
            //AllContentResult sup = await allContent.getAllContent();

            //AllShows allShows = new AllShows();
            //Shows sup = await allShows.getAllContent();

            //ShowDetails showDetails = new ShowDetails("XamarinShow");
            //ShowDetail sup = await showDetails.getAllContent();

            //VideoDetails showDetails = new VideoDetails("XamarinShow", "XamarinForms-101-Getting-to-Know-your-XAML-Workspace-Visual-Studio-2019-for-Mac");
            VideoDetails showDetails = new VideoDetails("XamarinShow", "XamRight-Richer-Safer-XAML-Coding-Experience--The-Xamarin-Show");
            SingleEpisode sup = await showDetails.getAllContent();

            Console.WriteLine("Value of sup : " + sup);
        }
    }
}
