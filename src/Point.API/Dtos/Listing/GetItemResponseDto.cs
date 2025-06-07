using Point.Core.Domain.Entities;

namespace Point.API.Dtos.Listing
{
    public sealed class GetItemResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public Category? Category { get; init; }
        public string? Description { get; init; }
        public List<Tag>? Tags { get; init; }
        public List<GetItemUnitResponseDto>? Units { get; set; }

        public void AddUnit(GetItemUnitResponseDto unit)
        {
            if (unit == null) return;
            if (Units == null) Units = [];
            Units.Add(unit);
        }
    }

    public sealed class GetItemUnitResponseDto
    {
        public int Id { get; init; }
        public Unit Unit { get; init; }
        public string? ItemCode { get; init; }
        public string? CostPriceCode { get; init; }
        public List<GetPriceResponseDto>? Prices { get; init; }
    }

    public sealed class  GetPriceResponseDto
    {
        public GetPriceTypeResponseDto PriceType { get; init; }
        public decimal Amount { get; init; }
    }

    public sealed class GetPriceTypeResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
