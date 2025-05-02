using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class Price : IEntities
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int PriceTypeId { get; set; }
    }
}
