using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BrewServe.Infrastructure.Middleware;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    private readonly ILogger<LoggingMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = Stopwatch.StartNew();
        await _next(context);
        stopWatch.Stop();
        _logger.LogInformation(
            $"Request {context.Request.Method} {context.Request.Path} executed in {stopWatch.ElapsedMilliseconds}ms");
    }
}