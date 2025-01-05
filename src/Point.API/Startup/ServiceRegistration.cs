using FluentValidation;
using MediatR;
using Point.API.Constants;
using Point.API.Conventions;
using Point.Infrastructure.Persistence;

namespace Point.API.Startup
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
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

            // Middlewares
            services.Scan(scan => scan.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(IMiddleware)))
                .AsSelf()
                .WithTransientLifetime());

            // Swagger
            services.RegisterSwagger();

            return services;
        }

        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddPointDomain(configuration.GetConnectionString(DomainContants.ConnectionStringName));

            return services;
        }
    }
}
