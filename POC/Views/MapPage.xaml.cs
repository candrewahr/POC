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
        public List<Brewery> BreweriesInProximity { get; set; }
        public MapViewModel viewModel;
        public Placemark UserPlacemark { get; set; }

        public MapPage()
        {
            BindingContext = viewModel = new MapViewModel(this);
            InitializeComponent();
            UserPlacemark = new Placemark(App.UserPlacemark);
            BreweriesInProximity = new List<Brewery>();
        }

        protected override async void OnAppearing()
        {
            if (App.UserPlacemark.Location == null)
            {
                var currentLocation = await App.RetrieveUserLocation();
                UserPlacemark = await App.ReverseGeocode(currentLocation);
            }
            BreweriesInProximity = await viewModel.FilterBreweries(UserPlacemark);
            viewModel.UpdateMapWithBreweryPins(BreweriesInProximity);
            BreweryMap.MapType = (MapType) Preferences.Get("defaultMapType", (int)MapType.Street);
            viewModel.MoveToCurrentLocation();
        }

        void OnMapSettingsButtonClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new MapSettingsView(BreweryMap, viewModel));
        }        
    }
}
