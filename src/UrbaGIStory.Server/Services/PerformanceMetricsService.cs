using UrbaGIStory.Server.DTOs.Responses;

namespace UrbaGIStory.Server.Services;

/// <summary>
/// Service for tracking and retrieving performance metrics.
/// </summary>
public class PerformanceMetricsService
{
    private static readonly List<RequestMetric> _requestMetrics = new();
    private static readonly List<QueryMetric> _queryMetrics = new();
    private static readonly object _lock = new();
    private static DateTime _lastResetTime = DateTime.UtcNow;

    /// <summary>
    /// Records a request metric.
    /// </summary>
    public static void RecordRequest(string method, string path, int statusCode, long durationMs)
    {
        lock (_lock)
        {
            _requestMetrics.Add(new RequestMetric
            {
                Method = method,
                Path = path,
                StatusCode = statusCode,
                DurationMs = durationMs,
                Timestamp = DateTime.UtcNow
            });

            // Keep only last 1000 requests
            if (_requestMetrics.Count > 1000)
            {
                _requestMetrics.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// Records a database query metric.
    /// </summary>
    public static void RecordQuery(long durationMs, string? query = null)
    {
        lock (_lock)
        {
            _queryMetrics.Add(new QueryMetric
            {
                DurationMs = durationMs,
                Query = query,
                Timestamp = DateTime.UtcNow
            });

            // Keep only last 1000 queries
            if (_queryMetrics.Count > 1000)
            {
                _queryMetrics.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// Gets current performance metrics.
    /// </summary>
    public PerformanceMetricsResponse GetMetrics()
    {
        lock (_lock)
        {
            var recentRequests = _requestMetrics
                .Where(r => r.Timestamp >= DateTime.UtcNow.AddHours(-1))
                .ToList();

            var recentQueries = _queryMetrics
                .Where(q => q.Timestamp >= DateTime.UtcNow.AddHours(-1))
                .ToList();

            var recentErrors = _requestMetrics
                .Where(r => r.StatusCode >= 400 && r.Timestamp >= DateTime.UtcNow.AddHours(-1))
                .ToList();

            return new PerformanceMetricsResponse
            {
                Requests = new RequestMetrics
                {
                    TotalCount = recentRequests.Count,
                    AverageDurationMs = recentRequests.Any() 
                        ? recentRequests.Average(r => r.DurationMs) 
                        : 0,
                    SlowRequestCount = recentRequests.Count(r => r.DurationMs > 2000)
                },
                Database = new DatabaseMetrics
                {
                    TotalQueryCount = recentQueries.Count,
                    AverageQueryDurationMs = recentQueries.Any() 
                        ? recentQueries.Average(q => q.DurationMs) 
                        : 0,
                    SlowQueryCount = recentQueries.Count(q => q.DurationMs > 2000)
                },
                Errors = new ErrorMetrics
                {
                    TotalErrorCount = _requestMetrics.Count(r => r.StatusCode >= 400),
                    ErrorsLastHour = recentErrors.Count
                },
                System = new SystemMetrics
                {
                    ServerTimeUtc = DateTime.UtcNow,
                    UptimeSeconds = LogsService.GetUptimeSeconds()
                }
            };
        }
    }

    private class RequestMetric
    {
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public DateTime Timestamp { get; set; }
    }

    private class QueryMetric
    {
        public long DurationMs { get; set; }
        public string? Query { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

