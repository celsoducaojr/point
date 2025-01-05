using Point.API.Middlewares;

namespace Point.API.Startup
{
    public static class MiddlewareConfig
    {
        public static WebApplication UseMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }
    }
}
