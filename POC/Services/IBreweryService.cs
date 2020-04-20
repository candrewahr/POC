using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POC.Models;

namespace POC.Services
{
    public interface IBreweryService
    {

       Task<List<Brewery>> RefreshDataAsync();






    }
}
