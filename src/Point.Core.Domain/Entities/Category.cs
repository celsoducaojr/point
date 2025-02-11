﻿using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class Category : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public string? Name { get; set; }
    }
}
