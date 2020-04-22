using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using POC.ViewModels;

namespace POC.Views
{
    public partial class MapSettingsView : Rg.Plugins.Popup.Pages.PopupPage
    {
        Map _breweryMap;
        MapViewModel viewModel;
        public MapSettingsView(Map breweryMap)
        {
            _breweryMap = breweryMap;
            InitializeComponent();
            BindingContext = viewModel = new MapViewModel();
        }

        //onclickedevents for changing map settings...


        void OnMapTypeButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
            {
                case "Street":
                    _breweryMap.MapType = MapType.Street;
                    //TODO: PASS these settings back to the view and allow for it to make the updates to the local map.
                    break;
                case "Satellite":
                    _breweryMap.MapType = MapType.Satellite;
                    break;
                case "Hybrid":
                    _breweryMap.MapType = MapType.Hybrid;
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

        public void OnApplySettingsButtonClicked(object sender, EventArgs e)
        {
            viewModel.MapSettingsUpdated(_breweryMap);
        }

    }
}
