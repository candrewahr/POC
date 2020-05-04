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

namespace POC.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Brewery> NearbyBreweries { get; set; }
        public Command LoadItemsCommand { get; set; }
        private ItemsPage _itemsPage { get; set; }
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

        public ItemsViewModel(ItemsPage itemsPage)
        {
            _itemsPage = itemsPage;
            Title = "Nearby Breweries";
            NearbyBreweries = new ObservableCollection<Brewery>();
            _breweryService = new BreweryService();
            _geocoder = new Geocoder();
            InitMenuPage();           
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                NearbyBreweries.Clear();
                var nearestBreweries = (from brewery in Breweries
                               let geo = new Location(ConvertToDouble(brewery.Latitude), ConvertToDouble(brewery.Longitude))
                               orderby Location.CalculateDistance(geo, App.CurrentUserLocation, DistanceUnits.Miles)
                               select brewery).Take(10);

                foreach(var brewery in nearestBreweries)
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
            FilterBreweryListByProximity();
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