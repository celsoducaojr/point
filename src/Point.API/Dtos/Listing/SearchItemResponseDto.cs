using Point.Core.Domain.Entities;

namespace Point.API.Dtos.Listing
{
    public sealed class SearchItemResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public Category? Category { get; init; }
        public string? Description { get; init; }
        public List<Tag>? Tags { get; init; }
        public List<SearchItemUnitResponseDto>? Units { get; set; }

        public void AddUnit(SearchItemUnitResponseDto unit)
        {
            if (unit == null) return;
            if (Units == null) Units = [];
            Units.Add(unit);
        }
    }

    public sealed class SearchItemUnitResponseDto
    {
        public int Id { get; init; }
        public Unit Unit { get; init; }
        public string? ItemCode { get; init; }
        public string? CostPriceCode { get; init; }
        public List<SearchPriceResponseDto>? Prices { get; init; }
    }

    public sealed class  SearchPriceResponseDto
    {
        public SearchPriceTypeResponseDto PriceType { get; init; }
        public decimal Amount { get; init; }
    }

    public sealed class SearchPriceTypeResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayIndex { get; set; }
    }
}
