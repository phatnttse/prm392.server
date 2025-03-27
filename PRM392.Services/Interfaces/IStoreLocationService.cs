using PRM392.Repositories.Models;
using PRM392.Services.DTOs.StoreLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface IStoreLocationService
    {
        Task<ApplicationResponse> AddNewLocation(StoreLocationDTO storeLocationDTO);
        Task<ApplicationResponse> DeleteLocationAsync(decimal? latitute, decimal? longitute);
        Task<ApplicationResponse> GetStoreLocationsAsync();
    }
}
