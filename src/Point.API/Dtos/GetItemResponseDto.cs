using Point.Core.Domain.Entities;

namespace Point.API.Dtos
{
    public sealed class GetItemResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public Category? Category { get; init; }
        public List<GetItemTagResponseDto>? Tags { get; init; }
    }

    public sealed class GetItemTagResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }

}
