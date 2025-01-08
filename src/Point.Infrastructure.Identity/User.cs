using Microsoft.AspNetCore.Identity;

namespace Point.Infrastructure.Identity
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
