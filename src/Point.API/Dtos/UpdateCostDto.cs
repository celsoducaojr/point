namespace Point.API.Dtos
{
    public sealed record UpdateCostDto(
        decimal InitialAmount,
        decimal FinalAmount);
}
