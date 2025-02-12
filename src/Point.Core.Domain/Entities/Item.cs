using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class Item : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public List<ItemUnit>? Units { get; set; }
        public List<ItemTag>? Tags { get; set; }
    }
}
