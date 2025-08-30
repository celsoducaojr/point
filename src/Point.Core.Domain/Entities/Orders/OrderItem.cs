using Point.Core.Domain.Contracts.Entities;
using Point.Core.Domain.Enums;

namespace Point.Core.Domain.Entities.Orders
{
    public class OrderItem : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public int ItemUnitId { get; set; }
        public string ItemName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public OrderItemStatus Status {  get; set; }
    }
}
