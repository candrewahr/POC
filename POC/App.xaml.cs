using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using POC.Services;
using POC.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POC
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        public static bool UseMockDataStore = true;
        public static Location UserLocation = new Location();

        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AzureDataStore>();
            MainPage = new MainPage();
        }

        public static async Task<Location> RetrieveUserLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                var UserLocation = await Geolocation.GetLocationAsync(request) ?? await Geolocation.GetLastKnownLocationAsync();
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

            return UserLocation;
        }


        protected async override void OnStart()
        {
            UserLocation = await RetrieveUserLocation();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
