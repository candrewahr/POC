﻿using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using POC.Services;
using POC.Views;
using System.Threading.Tasks;
using System.Linq;

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
        public static Location CurrentUserLocation = new Location();
        public static Placemark UserPlacemark = new Placemark();

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
                var request = new GeolocationRequest(GeolocationAccuracy.Default, new TimeSpan(0, 0, 0, 0, 500));
                var UserLocation = await Geolocation.GetLocationAsync(request);
                if(UserLocation!= null)
                {
                    CurrentUserLocation = UserLocation;
                    return UserLocation;
                }
                else
                {
                    var lastKnownLocation = await Geolocation.GetLastKnownLocationAsync();
                    return lastKnownLocation;
                }
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

            return null;
        }

        public static async Task<Placemark> ReverseGeocode(Location location)
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(location);
                Placemark userPlacemark = placemarks?.FirstOrDefault();

                //Try to get location from last known if the initial Location fails...
                //Added to address some issues with getting location on start up
                if (userPlacemark.Location == null)
                {
                    placemarks = await Geocoding.GetPlacemarksAsync(await Geolocation.GetLastKnownLocationAsync());
                    userPlacemark = placemarks?.FirstOrDefault();
                }
                return userPlacemark;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }

            return null;
        }

        protected async override void OnStart()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (!status.Equals(PermissionStatus.Granted))
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status.Equals(PermissionStatus.Granted))
            {
                var currentLocation = await RetrieveUserLocation();
                UserPlacemark = await ReverseGeocode(currentLocation);
            }
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
