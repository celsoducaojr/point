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
        public string? PriceCode { get; set; }
        public List<Price>? Prices { get; set; }
        public CostReference? CostReference { get; set; }
        public bool IsDeleted { get; set; }
    }
}
