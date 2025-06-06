﻿using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities.Orders
{
    public class Customer : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public string Name { get; set; }
    }
}
