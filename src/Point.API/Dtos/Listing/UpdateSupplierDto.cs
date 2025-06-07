namespace Point.API.Dtos.Listing
{
    public sealed record UpdateSupplierDto(
        string Name,
        string? Remarks,
        List<int>? Tags);
}
