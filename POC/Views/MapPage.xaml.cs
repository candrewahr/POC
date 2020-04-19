using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace POC.Views
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            map.IsShowingUser = true;
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            if (location == null)
            {
                location = await Geolocation.GetLastKnownLocationAsync(); 
            }

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));

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
