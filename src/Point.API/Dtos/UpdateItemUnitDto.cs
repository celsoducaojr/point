namespace Point.API.Dtos
{
    public sealed record UpdateItemUnitDto(
        int ItemId,
        int UnitId,
        string? ItemCode,
        decimal RetailPrice,
        decimal WholeSalePrice,
        string? PriceCode,
        string? Remarks);
}
