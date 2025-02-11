using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class PriceVariation : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public decimal? Amount { get; set; }
        public decimal? Percentage { get; set; }
    }
}