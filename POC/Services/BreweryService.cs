using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using POC.MobileAppService.Models;
using Xamarin.Essentials;

namespace POC.Services
{
    public class BreweryService : IBreweryService
    {
        private const string BreweryApiURL = "https://api.openbrewerydb.org/breweries?";
        HttpClient _client;
        public List<Brewery> Breweries { get; private set; }

        public BreweryService()
        {
            _client = new HttpClient();
        }


        public async Task<List<Brewery>> GetBreweriesByCity(Placemark currentUserAddress)
        {

            Breweries = new List<Brewery>();
            var byCityUrl = BreweryApiURL + "by_city";
            var currentCity = currentUserAddress.Locality;
            currentCity.Replace(" ", "_");
            var uri = new Uri(byCityUrl + "=" + currentCity+"&per_page=50");
            
            try
            { 
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Breweries = JsonConvert.DeserializeObject<List<Brewery>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Breweries;
        }

        public async Task<List<Brewery>> GetBreweriesByPostalCode(Placemark currentUserAddress)
        {
            Breweries = new List<Brewery>();
            var byPostalCodeUrl = BreweryApiURL + "by_postal";
            var currentPostalCode = currentUserAddress.PostalCode;
            var uri = new Uri(byPostalCodeUrl + "=" + currentPostalCode);

            try
            {
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Breweries = JsonConvert.DeserializeObject<List<Brewery>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return Breweries;
        }

        //TODO: brewery lookup via search box... 
    }
}
