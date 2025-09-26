using Point.Core.Domain.Enums;

namespace Point.API.Dtos.Stocks
{
    public sealed record UpdateStockItemDto(
        StockUpdateType Type,
        int Quantity,
        string? Remarks);
}
