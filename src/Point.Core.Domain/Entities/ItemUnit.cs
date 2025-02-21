using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class ItemUnit : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public int ItemId { get; set; }
        public int UnitId { get; set; }
        public string? ItemCode { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public string? PriceCode { get; set; }
        public string? Remarks { get; set; }
        public PurchaseCost? PurchaseCost { get; set; }
        public bool IsDeleted { get; set; }
    }
}
