using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POC.MobileAppService.Models;
using POC.Models;
using Xamarin.Essentials;

namespace POC.Services
{
    public class YelpService
    {

        private const string _yelpApiUrl = "https://api.yelp.com/v3/businesses/";
        HttpClient _client;
        public List<YelpBusiness> Breweries { get; private set; }
        public YelpService()
        {
            _client = new HttpClient();
        }


        public async Task<List<YelpBusiness>> GetNearestYelpBreweries()
        {
            var request = WebRequest.Create(_yelpApiUrl + "search?term=breweries&latitude=" + App.CurrentUserLocation.Latitude + "&longitude=" + App.CurrentUserLocation.Longitude);
            request.Method = "GET";
            request.Headers.Add("Cache-Control", "no_cache");
            request.Headers.Add("Authorization", "Bearer VEqg75j7DtU6tlEy79lxdc1qlFUtDoPQ7xNSghZaVmcsSj0LnheZaM9pQfe4k4S6z--DDirPZyUtl5JU9IhjjHwLDZ6Z_BIzytT4bz8g4RmlyziSScZFC7dssiC3XnYx");
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            var stream = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            var breweriesJson = JObject.Parse(stream.ReadToEnd());

            var breweriesToMap = breweriesJson["businesses"];

            var Breweries = new List<YelpBusiness>();

            foreach (var brewery in breweriesToMap)
            {
                var breweryToAdd = new YelpBusiness()
                {
                    ID = brewery["id"]?.ToString(),
                    Name = brewery["name"]?.ToString(),
                    Location = brewery["coordinates"]?.ToObject<Location>(),
                    BreweryAddress = brewery["location"]?.ToObject<Address>(),
                    Url = brewery["url"]?.ToObject<Uri>(),
                    Alias = brewery["alias"]?.ToString(),
                    ReviewCount = brewery["review_count"]?.ToString(),
                    Price = brewery["price"]?.ToString(),
                    Rating = brewery["rating"]?.ToString(),
                    DistanceToBusiness = brewery["distance"]?.ToString()
                };

                Breweries.Add(breweryToAdd);
            }

            return Breweries;
        }
    }
}
