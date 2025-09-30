using Point.Core.Domain.Entities.Stocks;

namespace Point.API.Dtos.Stocks
{
    public class SearchStockResponseDto
    {
        public int ItemUnitId { get; init; }
        public string ItemName { get; init; }
        public string CategoryName { get; init; }
        public string ItemUnitName { get; init; }
        public int Quantity { get; init; }
        public List<StockHistory> Histories { get; init; }

    }
}
