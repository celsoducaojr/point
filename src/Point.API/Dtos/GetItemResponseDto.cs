using Point.Core.Domain.Entities;

namespace Point.API.Dtos
{
    public sealed class GetItemResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public string? Category { get; init; }
        public List<string>? Tags { get; init; }
    }
}
