using Point.Core.Domain.Enums;

namespace Point.API.Dtos.Stocks
{
    public sealed record UpdateStockItemDto(
        StockHistoryType Type,
        int Quantity,
        string? Remarks);
}
