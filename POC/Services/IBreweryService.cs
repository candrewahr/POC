using System.Collections.Generic;
using System.Threading.Tasks;
using POC.MobileAppService.Models;
using Xamarin.Essentials;

namespace POC.Services
{
    public interface IBreweryService
    {
        Task<List<Brewery>> GetBreweriesByCity(Placemark currentUserAddress);

        Task<List<Brewery>> GetBreweriesByState(Placemark currentUserAddress);

        Task<List<Brewery>> GetBreweriesByPostalCode(Placemark currentUserAddress);


    }
}
