using Microsoft.EntityFrameworkCore;
using PRM392.Repositories.Base;
using PRM392.Repositories.DbContext;
using PRM392.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories
{
    public class ProductImageRepository : GenericRepository<ProductImage>
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    
        public async Task<List<ProductImage>> AddRangeAsync(List<ProductImage> images)
        {
            await _context.ProductImages.AddRangeAsync(images);
            return images;
        }

    }
}
