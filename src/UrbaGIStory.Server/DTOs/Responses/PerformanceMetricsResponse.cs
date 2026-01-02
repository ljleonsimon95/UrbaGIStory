namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for system performance metrics.
/// </summary>
public class PerformanceMetricsResponse
{
    /// <summary>
    /// Request metrics.
    /// </summary>
    public RequestMetrics Requests { get; set; } = new();

    /// <summary>
    /// Database query metrics.
    /// </summary>
    public DatabaseMetrics Database { get; set; } = new();

    /// <summary>
    /// Error metrics.
    /// </summary>
    public ErrorMetrics Errors { get; set; } = new();

    /// <summary>
    /// System information.
    /// </summary>
    public SystemMetrics System { get; set; } = new();
}

/// <summary>
/// Request performance metrics.
/// </summary>
public class RequestMetrics
{
    /// <summary>
    /// Total number of requests.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Average request duration in milliseconds.
    /// </summary>
    public double AverageDurationMs { get; set; }

    /// <summary>
    /// Number of slow requests (> 2 seconds).
    /// </summary>
    public int SlowRequestCount { get; set; }
}

/// <summary>
/// Database query performance metrics.
/// </summary>
public class DatabaseMetrics
{
    /// <summary>
    /// Total number of queries executed.
    /// </summary>
    public int TotalQueryCount { get; set; }

    /// <summary>
    /// Average query duration in milliseconds.
    /// </summary>
    public double AverageQueryDurationMs { get; set; }

    /// <summary>
    /// Number of slow queries (> 2 seconds).
    /// </summary>
    public int SlowQueryCount { get; set; }
}

/// <summary>
/// Error metrics.
/// </summary>
public class ErrorMetrics
{
    /// <summary>
    /// Total number of errors.
    /// </summary>
    public int TotalErrorCount { get; set; }

    /// <summary>
    /// Number of errors in the last hour.
    /// </summary>
    public int ErrorsLastHour { get; set; }
}

/// <summary>
/// System metrics.
/// </summary>
public class SystemMetrics
{
    /// <summary>
    /// Server time (UTC).
    /// </summary>
    public DateTime ServerTimeUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Application uptime in seconds.
    /// </summary>
    public long UptimeSeconds { get; set; }
}

