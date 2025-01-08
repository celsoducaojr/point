using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Point.Infrastructure.Identity
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) 
        : IdentityDbContext<User>(options)
    {

    }
}
