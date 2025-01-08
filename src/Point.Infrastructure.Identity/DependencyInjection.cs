using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Point.Infrastructure.Identity;
using System.Data;

namespace Point.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityDomain(this IServiceCollection services, string? connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "ConnectionString cannot be null.");
            }

            services.AddScoped<UserDbContext>();

            services.AddDbContext<UserDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddTransient<IDbConnection>((sp) => new MySqlConnection(connectionString));

            return services;
        }
    }
}
