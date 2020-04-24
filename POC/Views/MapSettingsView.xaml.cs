using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using POC.ViewModels;
using Xamarin.Essentials;
using Map = Xamarin.Forms.Maps.Map;
using POC.Models;
using System.Threading.Tasks;

namespace POC.Views
{
    public partial class MapSettingsView : Rg.Plugins.Popup.Pages.PopupPage
    {
        private Map _breweryMap;
        private MapViewModel _viewModel;
        public MapSettingsView(Map breweryMap, MapViewModel viewModel)
        {
            _breweryMap = breweryMap;
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
            SetMapTypeButtonEnabledProperties(_breweryMap.MapType);
            SetSearchSettingsButtonEnabledProperties((Enumerations.MapSearchType)Preferences.Get("defaultMapSearchType",
                (int)Enumerations.MapSearchType.City));
            this.CloseWhenBackgroundIsClicked = true;
        }

        //onclickedevents for changing map settings...


        /// <summary>
        /// Used to set the enabled properties on Map Type buttons in order to simulate radio button functionality.
        /// </summary>
        /// <param name="mapType"></param>
        public void SetMapTypeButtonEnabledProperties(MapType mapType)
        {
            if (mapType == MapType.Street)
            {
                Preferences.Set("defaultMapType", (int)MapType.Street);
                StreetButton.IsEnabled = false;
                SatteliteButton.IsEnabled = true;
                HybridButton.IsEnabled = true;
            }
            if (mapType == MapType.Satellite)
            {
                Preferences.Set("defaultMapType", (int)MapType.Satellite);
                SatteliteButton.IsEnabled = false;
                HybridButton.IsEnabled = true;
                StreetButton.IsEnabled = true;
            }
            if (mapType == MapType.Hybrid)
            {
                Preferences.Set("defaultMapType", (int)MapType.Hybrid);
                HybridButton.IsEnabled = false;
                StreetButton.IsEnabled = true;
                SatteliteButton.IsEnabled = true;
            }
        }

        public void SetSearchSettingsButtonEnabledProperties(Enumerations.MapSearchType mapSearchType)
        {
            if (mapSearchType == Enumerations.MapSearchType.City)
            {
                CityButton.IsEnabled = false;
                StateButton.IsEnabled = true;
                PostalCodeButton.IsEnabled = true;
            }
            if (mapSearchType == Enumerations.MapSearchType.State)
            {
                StateButton.IsEnabled = false;
                CityButton.IsEnabled = true;
                PostalCodeButton.IsEnabled = true;
            }
            if (mapSearchType == Enumerations.MapSearchType.PostalCode)
            {
                PostalCodeButton.IsEnabled = false;
                CityButton.IsEnabled = true;
                StateButton.IsEnabled = true;
            }
        }

        void OnSearchSettingsButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
            {
                case "City":
                    Preferences.Set("defaultMapSearchType", (int)Enumerations.MapSearchType.City);
                    MapSettingsUpdated();
                    SetSearchSettingsButtonEnabledProperties(Enumerations.MapSearchType.City);
                    break;
                case "State":
                    Preferences.Set("defaultMapSearchType", (int)Enumerations.MapSearchType.State);
                    MapSettingsUpdated();
                    SetSearchSettingsButtonEnabledProperties(Enumerations.MapSearchType.State);
                    break;
                case "Postal Code":
                    Preferences.Set("defaultMapSearchType", (int)Enumerations.MapSearchType.PostalCode);
                    MapSettingsUpdated();
                    SetSearchSettingsButtonEnabledProperties(Enumerations.MapSearchType.PostalCode);
                    break;
            }
        }

        void OnMapTypeButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
            {
                case "Street":
                    _breweryMap.MapType = MapType.Street;
                    SetMapTypeButtonEnabledProperties(_breweryMap.MapType);
                    break;
                case "Satellite":
                    _breweryMap.MapType = MapType.Satellite;
                    SetMapTypeButtonEnabledProperties(_breweryMap.MapType);
                    break;
                case "Hybrid":
                    _breweryMap.MapType = MapType.Hybrid;
                    SetMapTypeButtonEnabledProperties(_breweryMap.MapType);
                    break;
            }
        }

        public void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            double zoomLevel = e.NewValue;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if (_breweryMap.VisibleRegion != null)
            {
                _breweryMap.MoveToRegion(new MapSpan(_breweryMap.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        public async Task<TaskStatus> MapSettingsUpdated()
        {
            try
            {
                var breweriesInProximity = await _viewModel.FilterBreweries(App.UserPlacemark);
                _viewModel.UpdateMapWithBreweryPins(breweriesInProximity);
                _viewModel.MoveToCurrentLocation();
                return TaskStatus.RanToCompletion;
            }
            catch
            {
                return TaskStatus.Faulted;
            }
        }
    }
}
