using System;
using POC.MobileAppService.Models;
using Xamarin.Essentials;

namespace POC.Models
{
    public class YelpBusiness
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public Uri Url { get; set; }
        public string ReviewCount { get; set; }
        public string Rating { get; set; }
        public Location Location { get; set; }
        public string Price { get; set; }
        public Address BreweryAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string DistanceToBusiness { get; set; }        
    }
}

