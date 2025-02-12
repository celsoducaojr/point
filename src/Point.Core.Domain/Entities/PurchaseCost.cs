using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class PurchaseCost : IEntities, IAuditable
    {
        public int Id { get; set; } // PriceId, ForeignKey
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public decimal InitialAmount { get;set; }
        public decimal FinalAmount { get; set; }
        public List<DiscountVariation>? Variations { get; set; }

        
        // Navigation property to ItemUnit
        // One-to-one relationship reference
        public ItemUnit ItemUnit { get; set; }
    }
}
