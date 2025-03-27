using Microsoft.EntityFrameworkCore;
using PRM392.Repositories.Base;
using PRM392.Repositories.DbContext;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories
                .Where(c => c.ActiveFlag == (byte)ActiveFlag.Active)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByCodeAsync(string code)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Code!.ToUpper().Trim() == code.ToUpper().Trim());
        }
    }
}
