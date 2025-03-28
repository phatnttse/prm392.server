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

        public List<Product> GetFilteredAndSortedProducts(
            List<Product> products,
            string? sortBy = null,
            string? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null)
        {
            var query = products.AsQueryable();

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(categoryId))
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            // Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Sắp xếp theo tiêu chí được chọn
            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "category" => query.OrderBy(p => p.Category),
                _ => query
            };

            return query.ToList();
        }

    }
}
