using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using POC.Services;
using POC.MobileAppService.Models;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;

namespace POC.Views
{
    public partial class MapPage : ContentPage
    {
        BreweryService breweryService;
        List<Brewery> BreweriesInProximity;

        public MapPage()
        {
            InitializeComponent();
            breweryService = new BreweryService();
        }

        protected override async void OnAppearing()
        {
            BreweriesInProximity = await breweryService.GetBreweriesByCity(App.UserPlacemark);
            UpdateMapWithBreweryPins(BreweriesInProximity);
            MoveToCurrentLocation();
        }

        public void MoveToCurrentLocation()
        {
            map.IsShowingUser = true;

            try
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(App.CurrentUserLocation.Latitude, App.CurrentUserLocation.Longitude), Distance.FromMiles(1)));
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
                map.Pins.Add(pin);
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

        void OnMapSettingsButtonClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new MapSettingsView());
        }


    }
}
