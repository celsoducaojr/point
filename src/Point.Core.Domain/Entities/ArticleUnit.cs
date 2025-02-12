using Point.Core.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Point.Core.Domain.Entities
{
    public class ArticleUnit : IEntities
    {
        public int Id { get; set; }
        public string? Name { get; set; }    
    }
}
