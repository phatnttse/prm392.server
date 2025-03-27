using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Cart
{
    public class CreateUpdateCartItemDTO
    {
        public string? ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
