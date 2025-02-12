using Point.Core.Domain.Contracts.Entities;

namespace Point.Core.Domain.Entities
{
    public class Unit : IEntities
    {
        public int Id { get; set; }
        public string? Name { get; set; }    
    }
}
