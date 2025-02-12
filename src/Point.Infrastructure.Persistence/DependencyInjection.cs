using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Point.Core.Application.Contracts;
using Point.Core.Domain.Contracts.Repositories;
using Point.Infrastructure.Persistence.Repositories;
using System.Data;

namespace Point.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPointDomain(this IServiceCollection services, string? connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "ConnectionString cannot be null.");
            }

            services.AddScoped<IPointDbContext, PointDbContext>();

            services.AddDbContext<PointDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddTransient<IDbConnection>((sp) => new MySqlConnection(connectionString));

            // Repositories
            services.AddTransient<ISupplierRepository, SupplierRepository>();

            return services;
        }
    }
}
