using Microsoft.EntityFrameworkCore;
using PRM392.Repositories.DbContext;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories
{
    public class StoreLocationRepository : IStoreLocation
    {
        private readonly ApplicationDbContext _context;

        public StoreLocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StoreLocation> AddNewStoreLocation(StoreLocation location)
        {
            _context.StoreLocations.Add(location);
            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<List<StoreLocation>> GetAllStoreLocations()
        {
            return await _context.StoreLocations.ToListAsync();
        }

        public async Task<StoreLocation> RemoveStoreLocation(decimal latitude, decimal longitude)
        {
            var storeLocation = await _context.StoreLocations.FirstOrDefaultAsync(sl => sl.Latitude == latitude && sl.Longitude == longitude);
            if (storeLocation != null)
            {
                _context.StoreLocations.Remove(storeLocation);
                await _context.SaveChangesAsync();
            }
            return storeLocation;
        }
    }
}
