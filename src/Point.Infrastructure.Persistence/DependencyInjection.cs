using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Point.Core.Application.Contracts;
using Point.Core.Domain.Contracts.Repositories;
using Point.Infrastructure.Persistence.Contracts;
using Point.Infrastructure.Persistence.Repositories;

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

            services.AddTransient<IPointDbConnection>((sp) => new PointDbConnection(new MySqlConnection(connectionString)));

            // Repositories
            services.AddTransient<ISupplierRepository, SupplierRepository>();

            return services;
        }
    }
}
