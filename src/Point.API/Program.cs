using Point.API.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterInfrastructureDependencies(builder.Configuration)
    .RegisterDomainServices();

var app = builder.Build();

app.UseMiddlewares();

app.UseHttpsRedirection();

app.UseAuthorization();

app.ConfigureSwagger();

app.MapControllers();

app.Run();

