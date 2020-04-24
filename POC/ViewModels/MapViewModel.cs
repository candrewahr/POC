using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POC.MobileAppService.Models;
using POC.Models;
using POC.Services;
using POC.Views;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace POC.ViewModels
{
    public class MapViewModel
    {
        MapPage _mapPage;
        private BreweryService _breweryService;
        public List<Brewery> FilteredBreweries { get; set; }
        public MapViewModel(MapPage mapPage)
        {
            _mapPage = mapPage;
            _breweryService = new BreweryService();
        }


        public async Task<List<Brewery>> FilterBreweries(Placemark userPlaceMark)
        {
            switch ((Enumerations.MapSearchType)Preferences.Get("defaultMapSearchType", (int)Enumerations.MapSearchType.City))
            {
                case Enumerations.MapSearchType.City:
                    FilteredBreweries = await _breweryService.GetBreweriesByCity(userPlaceMark);
                    break;
                case Enumerations.MapSearchType.State:
                    FilteredBreweries = await _breweryService.GetBreweriesByState(userPlaceMark);
                    break;
                case Enumerations.MapSearchType.PostalCode:
                    FilteredBreweries = await _breweryService.GetBreweriesByPostalCode(userPlaceMark);
                    break;
            }

            return FilteredBreweries;
        }

        public void MoveToCurrentLocation()
        {
            _mapPage.BreweryMap.IsShowingUser = true;

            try
            {
                _mapPage.BreweryMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(App.CurrentUserLocation.Latitude, App.CurrentUserLocation.Longitude), Distance.FromMiles(1)));
            }
            catch (FeatureNotSupportedException fnsEx)
            {
            }
            catch (FeatureNotEnabledException fneEx)
            {
            }
            catch (PermissionException pEx)
            {
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateMapWithBreweryPins(List<Brewery> breweryList)
        {
            _mapPage.BreweryMap.Pins.Clear();
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
                _mapPage.BreweryMap.Pins.Add(pin);
            }
        }
    }
}
