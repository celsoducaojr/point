using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class Stock : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public int ItemUnitId { get;set; }  
        public int Quantity { get;set; }
        public int? Threshold { get; set; }
    }
}
