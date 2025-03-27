using PRM392.Services.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Order
{
    public class OrderDetailDTO
    {
        public ProductDTO? Product { get; set; }
        public int Quantity { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }
}
