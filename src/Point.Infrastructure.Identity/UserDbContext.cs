using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Point.Infrastructure.Identity.Domain.Entities;

namespace Point.Infrastructure.Identity
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) 
        : IdentityDbContext<User>(options)
    {

    }
}
