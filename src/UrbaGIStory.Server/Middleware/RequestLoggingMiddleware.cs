using System.Diagnostics;
using UrbaGIStory.Server.Services;

namespace UrbaGIStory.Server.Middleware;

/// <summary>
/// Middleware for logging HTTP request duration and performance metrics.
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private const int SlowRequestThresholdMs = 2000; // 2 seconds

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

        // Log request start
        _logger.LogInformation(
            "Request started: {Method} {Path} | TraceId: {TraceId}",
            requestMethod,
            requestPath,
            traceId);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var duration = stopwatch.ElapsedMilliseconds;
            var statusCode = context.Response.StatusCode;

            // Log request completion
            if (duration > SlowRequestThresholdMs)
            {
                _logger.LogWarning(
                    "Slow request detected: {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms | TraceId: {TraceId}",
                    requestMethod,
                    requestPath,
                    statusCode,
                    duration,
                    traceId);
            }
            else
            {
                _logger.LogInformation(
                    "Request completed: {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms | TraceId: {TraceId}",
                    requestMethod,
                    requestPath,
                    statusCode,
                    duration,
                    traceId);
            }

            // Record metrics for monitoring
            PerformanceMetricsService.RecordRequest(
                requestMethod,
                requestPath.ToString(),
                statusCode,
                duration);
        }
    }
}

