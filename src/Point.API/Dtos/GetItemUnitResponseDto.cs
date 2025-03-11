namespace Point.API.Dtos
{
    public sealed class GetItemUnitResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public string? Category { get; init; }
        public List<string>? Tags { get; init; }


        // ItemUnit details
        public string? Unit { get; set; }
        public string? ItemCode { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? WholesalePrice { get; set; }
        public string? PriceCode { get; set; }

    }
}
