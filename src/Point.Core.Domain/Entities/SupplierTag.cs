﻿using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class SupplierTag : IEntities
    {
        public int Id { get; set; }
        public int TagId { get; set; }
    }
}
