namespace Point.API.Dtos
{
    public sealed record UpdateSupplierDto(
        string Name,
        string? Remarks,
        List<int> Tags);
}
