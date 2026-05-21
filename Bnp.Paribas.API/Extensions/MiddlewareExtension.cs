using Bnp.Paribas.API.Middlewares;

namespace Bnp.Paribas.API.Extensions;

public static class MiddlewareExtension
{
    public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<LoggingMiddleware>();
        return app;
    }
}