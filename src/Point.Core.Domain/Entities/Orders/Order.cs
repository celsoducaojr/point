using Point.Core.Domain.Contracts.Entities;
using Point.Core.Domain.Enums;

namespace Point.Core.Domain.Entities.Orders
{
    public class Order : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public string Number { get; set; } // Order Number
        public int? CustomerId { get; set; }
        public decimal SubTotal { get; set; } 
        public decimal Discount { get; set; } 
        public decimal Total { get; set; } 
        public OrderStatus Status { get; set; }
        public List<OrderItem> Items { get; set; }
        public PaymentTerm? PaymentTerm { get; set; }
        public List<Payment>? Payments { get; set; }
    }
}
