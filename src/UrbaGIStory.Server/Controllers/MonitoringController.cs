using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Services;

namespace UrbaGIStory.Server.Controllers;

/// <summary>
/// Controller for system performance monitoring.
/// All endpoints require TechnicalAdministrator role.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "TechnicalAdministrator")]
public class MonitoringController : ControllerBase
{
    private readonly PerformanceMetricsService _metricsService;
    private readonly ILogger<MonitoringController> _logger;

    public MonitoringController(
        PerformanceMetricsService metricsService,
        ILogger<MonitoringController> logger)
    {
        _metricsService = metricsService;
        _logger = logger;
    }

    /// <summary>
    /// Gets current system performance metrics.
    /// </summary>
    /// <returns>Performance metrics including requests, database queries, errors, and system information</returns>
    [HttpGet("metrics")]
    [ProducesResponseType(typeof(PerformanceMetricsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetMetrics()
    {
        try
        {
            var metrics = _metricsService.GetMetrics();
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving performance metrics");
            throw;
        }
    }
}

