﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Point.Core.Application.Contracts;
using Point.Core.Domain.Contracts.Repositories;
using System.Data;

namespace Point.Order.Infra.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddListingDomain(this IServiceCollection services, string? connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "ConnectionString cannot be null.");
            }

            services.AddScoped<IPointContext, PointContext>();

            services.AddDbContext<PointContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddTransient<IDbConnection>((sp) => new MySqlConnection(connectionString));

            // Repositories
            services.AddTransient<IJobOrderRepository, ActivityRepository>();

            return services;
        }
    }
}
