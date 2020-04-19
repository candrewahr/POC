using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace POC.Views
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            double zoomLevel = e.NewValue;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if(map.VisibleRegion != null)
            {
                map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            //switch (button.Text)
      //      {
       //         case "Street":
     //               map.MapType = map.Street;
       //             break;
    //            case "Satellite":
     //               map.MapType = map.Satellite;
         //           break;
       //         case "Hybrid":
         //           map.MapType = map.Hybrid;
          //          break;
         //   }
        }


    }
}
