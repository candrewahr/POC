using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using POC.Views;
using POC.MobileAppService.Models;
using POC.Services;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using POC.Models;

namespace POC.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }
        public ObservableCollection<YelpBusiness> NearbyBreweries { get; set; }
        private ItemsPage _itemsPage { get; set; }
        private readonly BreweryService _breweryService;
        private ObservableCollection<YelpBusiness> _breweries;
        private Geocoder _geocoder;

        /// <summary>
        /// Observable Collection of Breweries for display on the Item Page.
        /// </summary>
        public ObservableCollection<YelpBusiness> Breweries
        {
            get => _breweries;
            set
            {
                _breweries = value;
                OnPropertyChanged();
            }
        }

        public ItemsViewModel(ItemsPage itemsPage)
        {
            _itemsPage = itemsPage;
            Title = "Nearby Breweries";
            _breweryService = new BreweryService();
            _geocoder = new Geocoder();
            Breweries = new ObservableCollection<YelpBusiness>();
            NearbyBreweries = new ObservableCollection<YelpBusiness>();
            InitMenuPage();           
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                NearbyBreweries.Clear();
                var nearestBreweries = (from brewery in Breweries
                                        let geo = new Location(brewery.Location.Latitude, brewery.Location.Longitude)
                                        orderby Location.CalculateDistance(geo, App.CurrentUserLocation, DistanceUnits.Miles)
                                        select brewery).Take(10);

                foreach (var brewery in nearestBreweries)
                {
                    NearbyBreweries.Add(brewery);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task InitMenuPage()
        {
            var yelpService = new YelpService();
            var nearbyBreweries = await yelpService.GetNearestYelpBreweries();
            Breweries = new ObservableCollection<YelpBusiness>(nearbyBreweries);
            await ExecuteLoadItemsCommand();
            return;
        }  

        public void FilterBreweryListByProximity()
        {
            return;
        }

        public double ConvertToDouble(string stringToConvert)
        {
            double.TryParse(stringToConvert, out var returnValue);
            return returnValue;
        }
    }
}