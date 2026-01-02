using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for filtering log entries.
/// </summary>
public class LogsFilterRequest
{
    /// <summary>
    /// Log level filter (Information, Warning, Error, Critical). Leave empty for all levels.
    /// </summary>
    public string? Level { get; set; }

    /// <summary>
    /// Start date for filtering logs (UTC).
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// End date for filtering logs (UTC).
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Search term to filter log messages.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Page number (1-based).
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
    public int PageSize { get; set; } = 50;
}

