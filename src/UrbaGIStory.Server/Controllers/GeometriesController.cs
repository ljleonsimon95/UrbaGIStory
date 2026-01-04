using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Exceptions;
using UrbaGIStory.Server.Services;

namespace UrbaGIStory.Server.Controllers;

/// <summary>
/// Controller for geometry management operations.
/// Note: Geometry creation and coordinate editing is done via QGIS only.
/// This controller handles metadata updates (name, description) and deletions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GeometriesController : ControllerBase
{
    private readonly GeometryService _geometryService;
    private readonly ILogger<GeometriesController> _logger;

    public GeometriesController(GeometryService geometryService, ILogger<GeometriesController> logger)
    {
        _geometryService = geometryService;
        _logger = logger;
    }

    #region GeoPoints

    /// <summary>
    /// Gets a list of point geometries.
    /// </summary>
    [HttpGet("points")]
    [ProducesResponseType(typeof(GeometriesListResponse<GeoPointResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGeoPoints([FromQuery] GeometriesFilterRequest request)
    {
        var result = await _geometryService.GetGeoPointsAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Gets a point geometry by ID.
    /// </summary>
    [HttpGet("points/{id}")]
    [ProducesResponseType(typeof(GeoPointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGeoPoint(Guid id)
    {
        try
        {
            var result = await _geometryService.GetGeoPointAsync(id);
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = $"GeoPoint with ID {id} not found" });
        }
    }

    /// <summary>
    /// Updates a point geometry's metadata (name, description only).
    /// Note: Geometry coordinates can only be modified via QGIS.
    /// </summary>
    [HttpPatch("points/{id}")]
    [ProducesResponseType(typeof(GeoPointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGeoPoint(Guid id, [FromBody] UpdateGeometryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _geometryService.UpdateGeoPointAsync(id, request);
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = $"GeoPoint with ID {id} not found" });
        }
    }

    /// <summary>
    /// Deletes a point geometry.
    /// Linked entities will have their GeoPointId set to NULL automatically.
    /// </summary>
    [HttpDelete("points/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGeoPoint(Guid id)
    {
        try
        {
            await _geometryService.DeleteGeoPointAsync(id);
            return NoContent();
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = $"GeoPoint with ID {id} not found" });
        }
    }

    #endregion

    #region GeoLines

    /// <summary>
    /// Gets a list of line geometries.
    /// </summary>
    [HttpGet("lines")]
    [ProducesResponseType(typeof(GeometriesListResponse<GeoLineResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGeoLines([FromQuery] GeometriesFilterRequest request)
    {
        var result = await _geometryService.GetGeoLinesAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Gets a line geometry by ID.
    /// </summary>
    [HttpGet("lines/{id}")]
    [ProducesResponseType(typeof(GeoLineResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGeoLine(Guid id)
    {
        try
        {
            var result = await _geometryService.GetGeoLineAsync(id);
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = $"GeoLine with ID {id} not found" });
        }
    }

    /// <summary>
    /// Updates a line geometry's metadata (name, description only).
    /// Note: Geometry coordinates can only be modified via QGIS.
    /// </summary>
    [HttpPatch("lines/{id}")]
    [ProducesResponseType(typeof(GeoLineResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGeoLine(Guid id, [FromBody] UpdateGeometryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _geometryService.UpdateGeoLineAsync(id, request);
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = $"GeoLine with ID {id} not found" });
        }
    }

    /// <summary>
    /// Deletes a line geometry.
    /// Linked entities will have their GeoLineId set to NULL automatically.
    /// </summary>
    [HttpDelete("lines/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGeoLine(Guid id)
    {
        try
        {
            await _geometryService.DeleteGeoLineAsync(id);
            return NoContent();
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = $"GeoLine with ID {id} not found" });
        }
    }

    #endregion

    #region GeoPolygons

    /// <summary>
    /// Gets a list of polygon geometries.
    /// </summary>
    [HttpGet("polygons")]
    [ProducesResponseType(typeof(GeometriesListResponse<GeoPolygonResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGeoPolygons([FromQuery] GeometriesFilterRequest request)
    {
        var result = await _geometryService.GetGeoPolygonsAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Gets a polygon geometry by ID.
    /// </summary>
    [HttpGet("polygons/{id}")]
    [ProducesResponseType(typeof(GeoPolygonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGeoPolygon(Guid id)
    {
        try
        {
            var result = await _geometryService.GetGeoPolygonAsync(id);
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = $"GeoPolygon with ID {id} not found" });
        }
    }

    /// <summary>
    /// Updates a polygon geometry's metadata (name, description only).
    /// Note: Geometry coordinates can only be modified via QGIS.
    /// </summary>
    [HttpPatch("polygons/{id}")]
    [ProducesResponseType(typeof(GeoPolygonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGeoPolygon(Guid id, [FromBody] UpdateGeometryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _geometryService.UpdateGeoPolygonAsync(id, request);
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = $"GeoPolygon with ID {id} not found" });
        }
    }

    /// <summary>
    /// Deletes a polygon geometry.
    /// Linked entities will have their GeoPolygonId set to NULL automatically.
    /// </summary>
    [HttpDelete("polygons/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGeoPolygon(Guid id)
    {
        try
        {
            await _geometryService.DeleteGeoPolygonAsync(id);
            return NoContent();
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = $"GeoPolygon with ID {id} not found" });
        }
    }

    #endregion
}

