using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class PriceReference : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public decimal InitialPrice { get;set; }
        public decimal FinalPrice { get; set; }
        public List<PriceVariation>? Variations { get; set; }

        
        // Navigation property to Price
        // One-to-one relationship reference
        public Price Price { get; set; }
    }
}
