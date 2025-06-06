﻿using Point.Core.Domain.Contracts.Entities;
using Point.Core.Domain.Enums;

namespace Point.Core.Domain.Entities.Orders
{
    public class Payment : IEntities, IAuditable
    {
        public int Id { get; set; } // PriceId, ForeignKey
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public decimal Amount { get; set; }
        public PaymentMode Mode { get; set; }
        public string? Reference { get; set; }
        public string? Remarks { get; set; }
    }
}
