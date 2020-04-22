using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using POC.ViewModels;

namespace POC.Views
{
    public partial class MapSettingsView : Rg.Plugins.Popup.Pages.PopupPage
    {
        private Map _breweryMap;
        private MapViewModel viewModel;
        public MapSettingsView(Map breweryMap)
        {
            _breweryMap = breweryMap;
            InitializeComponent();
            BindingContext = viewModel = new MapViewModel();
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
            if(mapType == MapType.Street)
            {
                StreetButton.IsEnabled = false;
                SatteliteButton.IsEnabled = true;
                HybridButton.IsEnabled = true;
            }
            if(mapType == MapType.Satellite)
            {
                SatteliteButton.IsEnabled = false;
                HybridButton.IsEnabled = true;
                StreetButton.IsEnabled = true;
            }
            if(mapType == MapType.Hybrid)
            {
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
            viewModel.MapSettingsUpdated(_breweryMap);
        }

    }
}
