using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class CostVariation : IEntities
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percentage { get; set; }
        public string? Remarks { get; set; }
    }
}