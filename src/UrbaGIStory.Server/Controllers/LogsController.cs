using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Services;

namespace UrbaGIStory.Server.Controllers;

/// <summary>
/// Controller for log viewing and export operations.
/// All endpoints require TechnicalAdministrator role.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "TechnicalAdministrator")]
public class LogsController : ControllerBase
{
    private readonly LogsService _logsService;
    private readonly ILogger<LogsController> _logger;

    public LogsController(LogsService logsService, ILogger<LogsController> logger)
    {
        _logsService = logsService;
        _logger = logger;
    }

    /// <summary>
    /// Gets filtered and paginated log entries.
    /// </summary>
    /// <param name="request">Filter and pagination options</param>
    /// <returns>Paginated list of log entries</returns>
    [HttpGet]
    [ProducesResponseType(typeof(LogsListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetLogs([FromQuery] LogsFilterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = _logsService.GetLogs(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving logs");
            throw;
        }
    }

    /// <summary>
    /// Exports logs as JSON.
    /// </summary>
    /// <param name="request">Filter options</param>
    /// <returns>JSON file with log entries</returns>
    [HttpGet("export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult ExportLogs([FromQuery] LogsFilterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var json = _logsService.ExportLogs(request);
            var fileName = $"logs_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
            
            return File(
                System.Text.Encoding.UTF8.GetBytes(json),
                "application/json",
                fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting logs");
            throw;
        }
    }
}

