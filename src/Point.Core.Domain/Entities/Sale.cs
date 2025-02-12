using Point.Core.Domain.Contracts.Entities;
using Point.Core.Domain.Entities;
using Point.Core.Domain.Entities.Enums;

namespace Point.Order.Core.Domain.Entities
{
    public class Sale : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public int? CustomerId { get; set; }
        public decimal? Discount { get; set; }
        public decimal Total { get; set; } 
        public SaleStatus Status { get; set; }
        public PaymentTerm PaymentTerm { get; set; }
        public List<SaleItem> Items { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
