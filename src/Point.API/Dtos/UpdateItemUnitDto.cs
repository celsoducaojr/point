using Point.Core.Application.Handlers;

namespace Point.API.Dtos
{
    public sealed record UpdateItemUnitDto(
        int ItemId,
        int UnitId,
        string? ItemCode,
        string? PriceCode,
        List<CreatePriceRequest>? Prices);
}
