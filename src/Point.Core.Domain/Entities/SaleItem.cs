using Point.Core.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Point.Core.Domain.Entities
{
    public class SaleItem : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public int ItemUnitId { get; set; }
        public int Quantity { get;set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal Total { get; set; }
    }
}
