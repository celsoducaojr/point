using Microsoft.AspNetCore.Identity;

namespace Point.Infrastructure.Identity.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
