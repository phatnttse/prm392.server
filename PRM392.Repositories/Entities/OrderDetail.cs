using PRM392.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Entities
{
    public class OrderDetail : BaseEntity
    {
        public string? OrderId { get; set; }
        public virtual Order? Order { get; set; }
        public string? ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public virtual decimal Discount { get; set; } = decimal.Zero;
    }
}
