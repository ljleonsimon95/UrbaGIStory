using System.Text.RegularExpressions;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;

namespace UrbaGIStory.Server.Services;

/// <summary>
/// Service for reading and filtering log entries from log files.
/// </summary>
public class LogsService
{
    private readonly ILogger<LogsService> _logger;
    private readonly string _logsDirectory;
    private static readonly DateTime _applicationStartTime = DateTime.UtcNow;

    public LogsService(ILogger<LogsService> logger, IConfiguration configuration)
    {
        _logger = logger;
        var logsPath = configuration["Logging:File:Path"] ?? "logs/urbagistory-.log";
        _logsDirectory = Path.GetDirectoryName(logsPath) ?? "logs";
    }

    /// <summary>
    /// Gets filtered and paginated log entries.
    /// </summary>
    public LogsListResponse GetLogs(LogsFilterRequest request)
    {
        try
        {
            var allLogs = ReadLogFiles();
            
            // Apply filters
            var filteredLogs = allLogs.AsQueryable();

            if (!string.IsNullOrEmpty(request.Level))
            {
                filteredLogs = filteredLogs.Where(log => 
                    log.Level.Equals(request.Level, StringComparison.OrdinalIgnoreCase));
            }

            if (request.FromDate.HasValue)
            {
                filteredLogs = filteredLogs.Where(log => log.Timestamp >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                filteredLogs = filteredLogs.Where(log => log.Timestamp <= request.ToDate.Value);
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLowerInvariant();
                filteredLogs = filteredLogs.Where(log => 
                    log.Message.ToLowerInvariant().Contains(searchTerm) ||
                    (log.Exception != null && log.Exception.ToLowerInvariant().Contains(searchTerm)));
            }

            var totalCount = filteredLogs.Count();
            
            // Apply pagination
            var paginatedLogs = filteredLogs
                .OrderByDescending(log => log.Timestamp)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new LogsListResponse
            {
                Logs = paginatedLogs,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading logs");
            throw;
        }
    }

    /// <summary>
    /// Exports logs as JSON.
    /// </summary>
    public string ExportLogs(LogsFilterRequest request)
    {
        var logsResponse = GetLogs(request);
        return System.Text.Json.JsonSerializer.Serialize(logsResponse.Logs, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    private List<LogEntryResponse> ReadLogFiles()
    {
        var logs = new List<LogEntryResponse>();

        if (!Directory.Exists(_logsDirectory))
        {
            return logs;
        }

        // Read all log files matching the pattern
        var logFiles = Directory.GetFiles(_logsDirectory, "urbagistory-*.log")
            .OrderByDescending(f => File.GetCreationTime(f))
            .Take(30); // Last 30 days

        foreach (var logFile in logFiles)
        {
            try
            {
                var fileLogs = ParseLogFile(logFile);
                logs.AddRange(fileLogs);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error reading log file: {LogFile}", logFile);
            }
        }

        return logs;
    }

    private List<LogEntryResponse> ParseLogFile(string filePath)
    {
        var logs = new List<LogEntryResponse>();
        
        // Serilog format: {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}
        var pattern = @"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3} [+-]\d{2}:\d{2})\s+\[(\w+)\]\s+\[(\d+)\]\s+(.+?)(?=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{2}|\z)";
        
        var content = File.ReadAllText(filePath);
        var matches = Regex.Matches(content, pattern, RegexOptions.Singleline | RegexOptions.Multiline);

        foreach (Match match in matches)
        {
            try
            {
                var timestampStr = match.Groups[1].Value;
                var level = match.Groups[2].Value;
                var threadId = match.Groups[3].Value;
                var message = match.Groups[4].Value.Trim();

                if (DateTime.TryParse(timestampStr, out var timestamp))
                {
                    var logEntry = new LogEntryResponse
                    {
                        Timestamp = timestamp,
                        Level = level,
                        Message = message,
                        Properties = new Dictionary<string, object>
                        {
                            { "ThreadId", threadId }
                        }
                    };

                    // Check if message contains exception
                    if (message.Contains("Exception") || message.Contains("Error"))
                    {
                        var exceptionMatch = Regex.Match(message, @"Exception:\s*(.+?)(?=\n|$)", RegexOptions.Singleline);
                        if (exceptionMatch.Success)
                        {
                            logEntry.Exception = exceptionMatch.Groups[1].Value.Trim();
                        }
                    }

                    logs.Add(logEntry);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error parsing log entry");
            }
        }

        return logs;
    }

    /// <summary>
    /// Gets application uptime in seconds.
    /// </summary>
    public static long GetUptimeSeconds()
    {
        return (long)(DateTime.UtcNow - _applicationStartTime).TotalSeconds;
    }
}

