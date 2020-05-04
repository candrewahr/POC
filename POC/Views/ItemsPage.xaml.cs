using System;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Xamarin.Forms;
using POC.Models;
using POC.ViewModels;
using POC.Services;
using System.Collections.ObjectModel;
using POC.MobileAppService.Models;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;

namespace POC.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        private readonly BreweryService _breweryService;
        private ObservableCollection<Brewery> _breweries;
        private Geocoder _geocoder;

        /// <summary>
        /// Observable Collection of Breweries for display on the Item Page.
        /// </summary>
        public ObservableCollection<Brewery> Breweries
        {
            get => _breweries;
            set
            {
                _breweries = value;
                OnPropertyChanged();
            }
        }


        public ItemsPage()
        {
            _breweryService = new BreweryService();
            _geocoder = new Geocoder();
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Item)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await InitMenuPage();
            await FilterBreweryListByProximity();
            if (viewModel.Items.Count == 0)
                viewModel.IsBusy = true;
        }

        public async Task InitMenuPage()
        {
            var breweriesByState = await _breweryService.GetBreweriesByState(App.UserPlacemark);
            Breweries = new ObservableCollection<Brewery>(breweriesByState);
            foreach (var brewery in Breweries)
            {
                if (brewery.Latitude == null || brewery.Longitude == null)
                {
                    IEnumerable<Position> breweryLatLong = await _geocoder.GetPositionsForAddressAsync(brewery.Street + ", " + brewery.City + ", " + brewery.State + " " + brewery.PostalCode);
                    var position = breweryLatLong.FirstOrDefault();
                    brewery.Latitude = position.Latitude.ToString();
                    brewery.Longitude = position.Longitude.ToString();
                }
            }
            return;
        }

        public async Task FilterBreweryListByProximity()
        {

           
            var nearestBrewery = Breweries.Select(x => new Location(x.Latitude, x.Longitude))
                                   .OrderBy(x => x.CalculateDistance(x))
                                   .First();
            return;
        }
    }
}