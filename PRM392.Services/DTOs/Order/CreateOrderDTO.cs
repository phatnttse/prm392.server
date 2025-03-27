using PRM392.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Order
{
    public class CreateOrderDTO
    {
        [Required]
        public required string BuyerName { get; set; }

        [Required]
        public required string BuyerPhone { get; set; }

        [Required]
        public required string BuyerAddress { get; set; }

        [Required]
        public required string BuyerEmail { get; set; }

        public string? Note { get; set; }

        [Required]
        public required PaymentMethod PaymentMethod { get; set; }

    }
}
