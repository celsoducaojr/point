using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class Price : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public string? ArticleCode { get; set; }
        public int ArticleUnitId { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public string? PriceCode { get; set; }
        public string? Remarks { get; set; }
        public PriceReference? References { get; set; } 
    }
}
