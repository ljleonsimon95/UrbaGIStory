using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Services;

namespace UrbaGIStory.Server.Controllers;

/// <summary>
/// Controller for database backup and restore operations.
/// All endpoints require TechnicalAdministrator role.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "TechnicalAdministrator")]
public class BackupController : ControllerBase
{
    private readonly BackupService _backupService;
    private readonly ILogger<BackupController> _logger;

    public BackupController(BackupService backupService, ILogger<BackupController> logger)
    {
        _backupService = backupService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new database backup.
    /// </summary>
    /// <returns>Backup information</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BackupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBackup()
    {
        try
        {
            var backup = await _backupService.CreateBackupAsync();
            return Ok(backup);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating backup");
            throw;
        }
    }

    /// <summary>
    /// Lists all available database backups.
    /// </summary>
    /// <returns>List of available backups</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BackupListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult ListBackups()
    {
        try
        {
            var backups = _backupService.ListBackups();
            return Ok(backups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing backups");
            throw;
        }
    }

    /// <summary>
    /// Restores the database from a backup file.
    /// </summary>
    /// <param name="request">Restore request options</param>
    /// <returns>Restore status</returns>
    [HttpPost("restore")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RestoreBackup([FromBody] RestoreRequest? request = null)
    {
        if (request == null)
        {
            return BadRequest(new { message = "RestoreRequest is required" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _backupService.RestoreBackupAsync(request);

            return Ok(new { message = "Database restored successfully", request.BackupId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring backup: {BackupId}", request.BackupId);
            throw;
        }
    }
}

