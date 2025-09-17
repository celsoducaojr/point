using Point.Core.Domain.Contracts.Entities;
using Point.Core.Domain.Enums;

namespace Point.Core.Domain.Entities.Stocks
{
    public class StockHistory : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public int? OrderItemId { get; set; }
        public int QuantityChanged { get; set; }    
        public int QuantityAfterChange { get; set; }
        public StockHistoryType Type { get; set; }
        public string? Remarks { get; set; }
    }
}
