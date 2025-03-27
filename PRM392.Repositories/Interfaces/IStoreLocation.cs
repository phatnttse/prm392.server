using PRM392.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Interfaces
{
    public interface IStoreLocation
    {
        Task<StoreLocation> AddNewStoreLocation(StoreLocation location);
        Task<StoreLocation> RemoveStoreLocation(decimal latitude, decimal longitude);
        Task<List<StoreLocation>> GetAllStoreLocations();
    }
}
