namespace Point.API.Dtos.Listing
{
    public sealed record UpdateItemDto(
        string Name,
        string? Description,
        int? CategoryId,
        List<int>? Tags);
}
