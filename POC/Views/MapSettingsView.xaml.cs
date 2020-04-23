using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using POC.ViewModels;
using Xamarin.Essentials;
using Map = Xamarin.Forms.Maps.Map;

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
            SetButtonEnabledProperties(_breweryMap.MapType);
        }

        //onclickedevents for changing map settings...
     
        void OnStreetButtonClicked(object sender, EventArgs e)
        {
            if (StreetButton.IsEnabled)
            {
                _breweryMap.MapType = MapType.Street;
                SetButtonEnabledProperties(_breweryMap.MapType);
            }
        }

        void OnSatteliteButtonClicked(object sender, EventArgs e)
        {
            if (SatteliteButton.IsEnabled)
            {
                _breweryMap.MapType = MapType.Satellite;
                SetButtonEnabledProperties(_breweryMap.MapType);
            }
        }

        void OnHybridButtonClicked(object sender, EventArgs e)
        {
            if (HybridButton.IsEnabled)
            {
                _breweryMap.MapType = MapType.Hybrid;
                SetButtonEnabledProperties(_breweryMap.MapType);
            }
        }

        public void SetButtonEnabledProperties(MapType mapType)
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

        public void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            double zoomLevel = e.NewValue;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if (_breweryMap.VisibleRegion != null)
            {
                _breweryMap.MoveToRegion(new MapSpan(_breweryMap.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        public void OnApplySettingsButtonClicked(object sender, EventArgs e)
        {
            _viewModel.MapSettingsUpdated(_breweryMap);
        }

    }
}
