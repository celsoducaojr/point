using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class ArticleTag : IEntities
    {
        public int Id { get; set; }
        public int TagId { get; set; }
    }
}
