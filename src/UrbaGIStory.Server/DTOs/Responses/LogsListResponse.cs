namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for paginated list of log entries.
/// </summary>
public class LogsListResponse
{
    /// <summary>
    /// List of log entries.
    /// </summary>
    public List<LogEntryResponse> Logs { get; set; } = new();

    /// <summary>
    /// Total number of log entries matching the filter.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

