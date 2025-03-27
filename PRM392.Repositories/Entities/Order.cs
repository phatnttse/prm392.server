using PRM392.Repositories.Base;
using PRM392.Repositories.Enums;
using System.ComponentModel.DataAnnotations;

namespace PRM392.Repositories.Entities
{
    public class Order : BaseEntity
    {
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public int OrderNumber { get; set; }

        public string? BuyerName { get; set; }

        public string? BuyerAddress { get; set; }

        public string? BuyerEmail { get; set; }

        public string? BuyerPhone { get; set; }

        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus? Status { get; set; }

        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod? PaymentMethod { get; set; }

        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus? PaymentStatus { get; set; }

        public decimal Amount { get; set; }

        public string? Note { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
