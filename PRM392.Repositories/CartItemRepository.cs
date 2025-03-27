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
    public class CartItemRepository : GenericRepository<CartItem>
    {
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CartItem?> GetCartItemByUserIdAndProductId(string userId, string productId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId && ci.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        public async Task<CartItem?> GetCartItemById(string id)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<CartItem>> GetCartItemsByUserId(string userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }

        public async Task ClearCartByUserId(string userId)
        {
             await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ForEachAsync(ci => _context.CartItems.Remove(ci));
        }
    }
}
