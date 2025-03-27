using PRM392.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Order
{
    public class OrderDTO
    {
        public string? Id { get; set; }
        public int? OrderCode { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerPhone { get; set; }
        public string? BuyerAddress { get; set; }
        public string? BuyerEmail { get; set; }
        public string? Note { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }
}
