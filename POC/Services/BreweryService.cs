using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using POC.MobileAppService.Models;

namespace POC.Services
{
    public class BreweryService : IBreweryService
    {
            HttpClient _client;

            public List<Brewery> Breweries { get; private set; }

            //TODO: split this url up into constants
            public string BreweryApiURLByCity = "https://api.openbrewerydb.org/breweries?by_city";

            public BreweryService()
            {
                _client = new HttpClient();
            }

            public async Task<List<Brewery>> GetBreweriesByCity(Address currentUserAddress)
            {
                Breweries = new List<Brewery>();

                var currentCity = currentUserAddress.City;
                currentCity.Replace(" ", "_");
                var uri = new Uri(BreweryApiURLByCity + "=" + currentCity);
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
    }
}
