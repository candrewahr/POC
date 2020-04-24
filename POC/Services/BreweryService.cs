using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using POC.MobileAppService.Models;
using Xamarin.Essentials;
using POC.Extensions;

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

        /// <summary>
        /// Retrieve a list of breweries by city.
        /// </summary>
        /// <param name="currentUserAddress"></param>
        /// <returns></returns>
        public async Task<List<Brewery>> GetBreweriesByCity(Placemark currentUserAddress)
        {
            Breweries = new List<Brewery>();

            //set city or locality name & do a replace on any potential spaces
            var byCityUrl = BreweryApiURL + "by_city";
            var currentCity = currentUserAddress.Locality;          
            currentCity.Replace(" ", "_");

            //url construction
            var page = 1;
            var uri = new Uri(byCityUrl + "=" + currentCity + "&per_page=50&page=");

            while (page <= 3)
            {
                try
                {
                    var response = await _client.GetAsync(uri + page.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var BreweryPayload = JsonConvert.DeserializeObject<List<Brewery>>(content);
                        if (BreweryPayload.Count > 0)
                        {
                            foreach (var brewery in BreweryPayload)
                            {
                                Breweries.Add(brewery);
                            }
                        }
                        else
                        {
                            break;
                        }

                        page++;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);
                }
            }

            return Breweries;
        }
        
        /// <summary>
        /// Retrieve a list of all breweries in a state with a limit of 500
        /// </summary>
        /// <param name="currentUserAddress"></param>
        /// <returns></returns>
        public async Task<List<Brewery>> GetBreweriesByState(Placemark currentUserAddress)
        {

            Breweries = new List<Brewery>();

            //set city or locality name & do a replace on any potential spaces
            var byStateUrl = BreweryApiURL + "by_state";
            var currentState = currentUserAddress.AdminArea;

            //if state is abbreviated, we need the entire state name... call a string extension to get it 
            if(currentState.Length == 2)
            {
                currentState = currentState.GetFullStateName();
            }

            currentState.Replace(" ", "_");

            //url construction
            var page = 1;
            var uri = new Uri(byStateUrl + "=" + currentState + "&per_page=40&page=");

            while (page <= 3)
            {
                try
                {
                    var response = await _client.GetAsync(uri + page.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var BreweryPayload = JsonConvert.DeserializeObject<List<Brewery>>(content);
                        if (BreweryPayload.Count > 0)
                        {
                            foreach (var brewery in BreweryPayload)
                            {
                                Breweries.Add(brewery);
                            }
                        }
                        else
                        {
                            break;
                        }

                        page++;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);
                }
            }

            return Breweries;
        }

        /// <summary>
        /// Retrieve a list of all breweries in a Postal Code
        /// </summary>
        /// <param name="currentUserAddress"></param>
        /// <returns></returns>
        public async Task<List<Brewery>> GetBreweriesByPostalCode(Placemark currentUserAddress)
        {

            Breweries = new List<Brewery>();

            //set city or locality name & do a replace on any potential spaces
            var byPostalUrl = BreweryApiURL + "by_postal";
            var currentPostalCode = currentUserAddress.PostalCode;
            currentPostalCode.Replace(" ", "_");

            //url construction
            var page = 1;
            var uri = new Uri(byPostalUrl + "=" + currentPostalCode + "&per_page=50&page=");

            while (page <= 3)
            {
                try
                {
                    var response = await _client.GetAsync(uri + page.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var BreweryPayload = JsonConvert.DeserializeObject<List<Brewery>>(content);
                        if (BreweryPayload.Count > 0)
                        {
                            foreach (var brewery in BreweryPayload)
                            {
                                Breweries.Add(brewery);
                            }
                        }
                        else
                        {
                            break;
                        }

                        page++;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);
                }
            }

            return Breweries;
        }

        //TODO: brewery lookup via search box... 
    }
}
