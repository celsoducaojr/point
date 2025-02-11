using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class Article : IEntities, IAuditable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public List<ArticleTag>? Tags { get; set; }
        public List<Price>? Prices { get; set; }
    }
}
