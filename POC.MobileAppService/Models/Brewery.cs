using System;
namespace POC.MobileAppService.Models
{
    public class Brewery
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public byte BreweryType { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public Uri Website { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    }

}
