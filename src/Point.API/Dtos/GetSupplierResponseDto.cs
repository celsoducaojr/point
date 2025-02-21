﻿namespace Point.API.Dtos
{
    public sealed class GetSupplierResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Remarks { get; init; }
        public List<string>? Tags { get; init; }
    }
}
