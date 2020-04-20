using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POC.Services
{
    public interface IBreweryService
    {

       Task<List<Brewery>> RefreshDataAsync();


    }
}
