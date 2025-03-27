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
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderByCodeAsync(int code)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.OrderCode == code)
                .FirstOrDefaultAsync();
        }
    }
}
