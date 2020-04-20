using System.Collections.Generic;
using System.Threading.Tasks;
using POC.MobileAppService.Models;
namespace POC.Services
{
    public interface IBreweryService
    {
        Task<List<Brewery>> GetBreweriesByCity(Address currentUserAddress);
    }
}
