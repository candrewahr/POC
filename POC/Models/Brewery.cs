using System;
namespace POC.MobileAppService.Models
{
    public class Brewery
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public byte BreweryType { get; set; }
        //public Address Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double LatitudeDouble { get; set; }
        public double LongitudeDouble { get; set; }
        public double Longitude { get; set; }
        public string Phone { get; set; }
        public string WebsiteUrl { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    }

}
