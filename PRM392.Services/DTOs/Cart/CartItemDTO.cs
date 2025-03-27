using PRM392.Services.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Cart
{
    public class CartItemDTO
    {
        public string? Id { get; set; }
        public ProductDTO? Product { get; set; }
        public int Quantity { get; set; }
    }
}
