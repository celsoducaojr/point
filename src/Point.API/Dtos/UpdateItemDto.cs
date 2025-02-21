namespace Point.API.Dtos
{
    public sealed record UpdateItemDto(
        string Name,
        string? Description,
        int? CategoryId,
        List<int>? Tags);
}
