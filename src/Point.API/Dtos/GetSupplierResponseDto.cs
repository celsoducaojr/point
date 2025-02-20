namespace Point.API.Dtos
{
    public sealed record GetSupplierResponseDto(
        int Id,
        DateTime LastModified,
        string Name,
        List<int> Tags);
}
