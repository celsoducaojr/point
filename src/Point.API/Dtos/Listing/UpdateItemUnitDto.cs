using Point.Core.Application.Handlers.Listing;

namespace Point.API.Dtos.Listing
{
    public sealed record UpdateItemUnitDto(
        int ItemId,
        int UnitId,
        string? ItemCode,
        string? PriceCode,
        List<CreatePriceRequest>? Prices);
}
