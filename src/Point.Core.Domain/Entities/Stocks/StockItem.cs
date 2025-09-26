using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities.Stocks
{
    public class StockItem : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public int ItemUnitId { get; set; }
        public int Quantity { get; set; }
        public List<StockHistory> Histories {get;set;}
    }
}
