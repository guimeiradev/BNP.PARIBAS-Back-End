using System.Diagnostics;
using System.Text;

namespace Bnp.Paribas.API.Middlewares;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = context.TraceIdentifier;
        var method = context.Request.Method;
        var path = context.Request.Path;
        var stopwatch = Stopwatch.StartNew();

        string? requestBody = null;
        string? queryParams = null;
        string? errorMessage = null;

        if (HttpMethods.IsPost(method) || HttpMethods.IsPut(method) || HttpMethods.IsPatch(method))
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        if (HttpMethods.IsGet(method) && context.Request.QueryString.HasValue)
            queryParams = context.Request.QueryString.Value;

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            throw;
        }
        finally
        {
            stopwatch.Stop();

            var logLevel = context.Response.StatusCode >= 500 ? LogLevel.Error
                         : context.Response.StatusCode >= 400 ? LogLevel.Warning
                         : LogLevel.Information;

            logger.Log(logLevel,
                "Http Request | TraceId: {TraceId} | {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms | Body: {Body} | QueryParams: {QueryParams} | Error: {Error}",
                traceId,
                method,
                path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                requestBody ?? "-",
                queryParams ?? "-",
                errorMessage ?? "-");
        }
    }
}
