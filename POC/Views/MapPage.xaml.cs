using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using System.Threading.Tasks;
using POC.Services;
using POC.MobileAppService.Models;

namespace POC.Views
{
    public partial class MapPage : ContentPage
    {
        BreweryService breweryService;
        Location UserLocation;
        
        public MapPage()
        {
            InitializeComponent();
            breweryService = new BreweryService();
        }

        protected override async void OnAppearing()
        {
            await MoveToCurrentLocation();
            var address = new Address()
            {
                City = "Raleigh"
            };
            await breweryService.GetBreweriesByCity(address);

        }

        public async Task MoveToCurrentLocation()
        {
            map.IsShowingUser = true;
            var request = new GeolocationRequest(GeolocationAccuracy.Default);
            var location = await Geolocation.GetLocationAsync(request);

            if (location == null)
            {
                location = await Geolocation.GetLastKnownLocationAsync();
            }

            try
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception               
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                //Handle a permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
                //Location may not be available because of latency issues...
            }
        }


        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            double zoomLevel = e.NewValue;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if (map.VisibleRegion != null)
            {
                map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }


        void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
              {
                  case "Street":
                    map.MapType = MapType.Street;
                    break;
                 case "Satellite":
                     map.MapType = MapType.Satellite;
                     break;
                 case "Hybrid":
                     map.MapType = MapType.Hybrid;
                    break;
            }
        }


    }
}
