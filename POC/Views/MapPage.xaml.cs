using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using POC.Services;
using POC.MobileAppService.Models;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using POC.ViewModels;
using Map = Xamarin.Forms.Maps;


namespace POC.Views
{
    public partial class MapPage : ContentPage
    {
        BreweryService breweryService;
        List<Brewery> BreweriesInProximity;
        MapViewModel viewModel;

        public MapPage()
        {
            BindingContext = viewModel = new MapViewModel();
            InitializeComponent();
            breweryService = new BreweryService();
        }

        protected override async void OnAppearing()
        {
            if (App.UserPlacemark.Location == null)
            {
                var currentLocation = await App.RetrieveUserLocation();
                await App.ReverseGeocode(currentLocation);
            }
            BreweriesInProximity = await breweryService.GetBreweriesByState(App.UserPlacemark);
            UpdateMapWithBreweryPins(BreweriesInProximity);
            MoveToCurrentLocation();
        }

        void OnMapSettingsButtonClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new MapSettingsView(BreweryMap));
        }

        public void MoveToCurrentLocation()
        {
            BreweryMap.IsShowingUser = true;

            try
            {
                BreweryMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(App.CurrentUserLocation.Latitude, App.CurrentUserLocation.Longitude), Distance.FromMiles(1)));
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

        public void UpdateMapWithBreweryPins(List<Brewery> breweryList)
        {
            var pin = new Pin();
            foreach (Brewery brewery in breweryList)
            {
                pin.Address = brewery.Street + ", " + brewery.City + ", " + brewery.State;
                pin.Label = brewery.Name;
                pin.Type = PinType.Place;

                //have to try to cast the lat and long to doubles in order to create a position object...
                double.TryParse(brewery.Latitude, out var latitudeDouble);
                double.TryParse(brewery.Longitude, out var longitudeDouble);
                pin.Position = new Position(latitudeDouble, longitudeDouble);
                BreweryMap.Pins.Add(pin);
            }
        }
    }
}
