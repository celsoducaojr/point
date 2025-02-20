﻿using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Point.API.Constants;
using Point.API.Conventions;
using Point.API.Dtos.Mapping;
using Point.Infrastructure.Identity;
using Point.Infrastructure.Identity.Domain.Entities;
using Point.Infrastructure.Persistence;
using System.Reflection;

namespace Point.API.Startup
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddPointDomain(configuration.GetConnectionString(PointDomainContants.ConnectionStringName));
            services.AddIdentityDomain(configuration.GetConnectionString(IdentityDomainConstants.ConnectionStringName));

            return services;
        }

        public static IServiceCollection RegisterPointDomainServices(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new LowercaseControllerNameConvention());
            });

            // Handlers
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            // Validators
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

            // Behaviors
            services.Scan(scan => scan.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            return services;
        }

        public static IServiceCollection RegisterIdentityDomainServices(this IServiceCollection services)
        {
            services
                .AddAuthorization()
                .AddAuthentication()
                .AddBearerToken(IdentityConstants.BearerScheme);

            services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddApiEndpoints();

            return services;
        }

        public static IServiceCollection RegisterCommonServices(this IServiceCollection services)
        {
            // Middlewares
            services.Scan(scan => scan.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(IMiddleware)))
                .AsSelf()
                .WithTransientLifetime());

            // Swagger
            services.RegisterSwagger();

            // Mapper
            services.AddMapster();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
