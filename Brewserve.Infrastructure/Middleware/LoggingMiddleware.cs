using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static System.Net.Mime.MediaTypeNames;

namespace Brewserve.Infrastructure.Middleware
{

    public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<LoggingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            var stopWatch = Stopwatch.StartNew();
           
            await _next(context);
            stopWatch.Stop();
            
            _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} executed in {stopWatch.ElapsedMilliseconds}ms");
        }

    }
}
