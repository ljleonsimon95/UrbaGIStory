using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbaGIStory.Server.Exceptions;

namespace UrbaGIStory.Server.Middleware;

/// <summary>
/// Global exception handler middleware that catches all unhandled exceptions
/// and returns them as ProblemDetails (RFC 7807) responses.
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Generate trace ID
        var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

        // Log the exception with full details (including stack trace)
        _logger.LogError(
            exception,
            "Unhandled exception occurred. TraceId: {TraceId}",
            traceId);

        // Create ProblemDetails response
        var problemDetails = CreateProblemDetails(context, exception, traceId);

        // Set response properties
        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/problem+json";

        // Serialize and write response
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(problemDetails, jsonOptions);
        await context.Response.WriteAsync(json);
    }

    private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception, string traceId)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path,
            Extensions = { ["traceId"] = traceId }
        };

        switch (exception)
        {
            case EntityNotFoundException notFoundEx:
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                problemDetails.Title = "Not Found";
                problemDetails.Status = (int)HttpStatusCode.NotFound;
                problemDetails.Detail = notFoundEx.Message;
                break;

            case ValidationException validationEx:
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                problemDetails.Title = "Validation Failed";
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                problemDetails.Detail = validationEx.Message;
                
                // Add validation errors to extensions
                if (validationEx.Errors.Any())
                {
                    problemDetails.Extensions["errors"] = validationEx.Errors;
                }
                break;

            case UnauthorizedAccessException:
                problemDetails.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                problemDetails.Title = "Unauthorized";
                problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                problemDetails.Detail = "You are not authorized to perform this action.";
                break;

            case ArgumentException argEx:
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                problemDetails.Title = "Bad Request";
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                problemDetails.Detail = argEx.Message;
                break;

            case ConcurrencyConflictException concurrencyEx:
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8";
                problemDetails.Title = "Concurrency Conflict";
                problemDetails.Status = (int)HttpStatusCode.Conflict;
                problemDetails.Detail = concurrencyEx.Message;
                
                // Add entity information to extensions
                if (!string.IsNullOrEmpty(concurrencyEx.EntityType))
                {
                    problemDetails.Extensions["entityType"] = concurrencyEx.EntityType;
                }
                if (concurrencyEx.Id != null)
                {
                    problemDetails.Extensions["entityId"] = concurrencyEx.Id;
                }
                break;

            case DbUpdateConcurrencyException concurrencyDbEx:
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8";
                problemDetails.Title = "Concurrency Conflict";
                problemDetails.Status = (int)HttpStatusCode.Conflict;
                problemDetails.Detail = "The entity has been modified by another user. Please refresh and try again.";
                break;

            case DbUpdateException dbEx:
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                problemDetails.Title = "Database Error";
                problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                
                // Do not expose database-specific error details
                problemDetails.Detail = _environment.IsDevelopment()
                    ? dbEx.GetBaseException().Message
                    : "An error occurred while processing your request. Please try again later.";
                break;

            default:
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                problemDetails.Title = "Internal Server Error";
                problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                
                // Only expose exception details in development
                problemDetails.Detail = _environment.IsDevelopment()
                    ? exception.Message
                    : "An error occurred while processing your request. Please try again later.";
                break;
        }

        return problemDetails;
    }
}

