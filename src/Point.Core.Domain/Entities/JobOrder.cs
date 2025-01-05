using Point.Core.Domain.Contracts.Entities;
using Point.Core.Domain.Entities;
using Point.Core.Domain.Entities.Enums;

namespace Point.Order.Core.Domain.Entities
{
    public class JobOrder : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public JobOrderStatus Status { get; set; }
    }
}
