using Point.API.Startup;
using Point.Infrastructure.Identity.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer();

builder.Services
    .RegisterInfrastructureDependencies(builder.Configuration)
    .RegisterPointDomainServices()
    .RegisterIdentityDomainServices()
    .RegisterCommonServices();

var app = builder.Build();

app.UseMiddlewares();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.ConfigureSwagger();

app.MapControllers();

app.Run();

