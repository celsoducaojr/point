using Point.Core.Domain.Entities;

namespace Point.API.Dtos
{
    public sealed class GetItemUnitResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public Category? Category { get; init; }
        public string? Description { get; init; }
        public List<Tag>? Tags { get; init; }


        // ItemUnit details
        public Unit Unit { get; set; }
        public string? ItemCode { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public string? PriceCode { get; set; }
        public string? Remarks { get; set; }
    }
}
