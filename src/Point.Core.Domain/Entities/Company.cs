using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class Company : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public string? Name { get; set; }
        public string? Notes { get; set; }
        public List<CompanyTag>? Tags { get; set; }
    }
}
