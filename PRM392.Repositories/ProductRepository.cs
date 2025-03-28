using Microsoft.EntityFrameworkCore;
using PRM392.Repositories.Base;
using PRM392.Repositories.DbContext;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Where(p => p.ActiveFlag == (byte)ActiveFlag.Active)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetListProductAfterFilterByPrice(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<List<Product>> GetListProductAfterFilterByCategory(string categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
        }
    }
}
