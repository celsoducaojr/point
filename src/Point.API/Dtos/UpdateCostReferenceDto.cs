namespace Point.API.Dtos
{
    public sealed record UpdateCostReferenceDto(
        decimal InitialAmount,
        decimal FinalAmount,
        List<UpdateDiscountVarationDto>? Variations);

    public sealed record UpdateDiscountVarationDto(
        decimal? Amount,
        decimal? Percentage,
        string? Remarks);
}
