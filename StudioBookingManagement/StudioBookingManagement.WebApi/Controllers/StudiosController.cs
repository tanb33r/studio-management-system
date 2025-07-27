using Microsoft.AspNetCore.Mvc;
using StudioBookingManagement.Application.DTOs.Studio;
using StudioBookingManagement.Application.Interfaces;

namespace StudioBookingManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudiosController : ControllerBase
{
    private readonly IStudioService _studioService;

    public StudiosController(IStudioService studioService)
    {
        _studioService = studioService;
    }
    [HttpGet]
    public async Task<IActionResult> GetStudios(CancellationToken cancellationToken)
    {
        try
        {
            var studios = await _studioService.GetAllStudiosAsync(cancellationToken);
            return Ok(studios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving studios", error = ex.Message });
        }
    }

    
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudio(int id, CancellationToken cancellationToken)
    {
        try
        {
            var studio = await _studioService.GetStudioByIdAsync(id, cancellationToken);
            if (studio == null)
            {
                return NotFound(new { message = $"Studio with ID {id} not found" });
            }

            return Ok(studio);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the studio", error = ex.Message });
        }
    }

    
    
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchStudios([FromQuery] string? area, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<StudioSummaryDto> studios;

            if (!string.IsNullOrWhiteSpace(area))
            {
                studios = await _studioService.GetStudiosByAreaAsync(area, cancellationToken);
            }
            else
            {
                studios = await _studioService.GetAllStudiosAsync(cancellationToken);
            }

            return Ok(studios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while searching studios", error = ex.Message });
        }
    }

    
    [HttpGet("nearby")]
    public async Task<IActionResult> GetNearbyStudios(
        [FromQuery] decimal lat,
        [FromQuery] decimal lng,
        [FromQuery] double radius = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (lat < -90 || lat > 90)
            {
                return BadRequest(new { message = "Latitude must be between -90 and 90" });
            }

            if (lng < -180 || lng > 180)
            {
                return BadRequest(new { message = "Longitude must be between -180 and 180" });
            }

            if (radius <= 0 || radius > 100)
            {
                return BadRequest(new { message = "Radius must be between 0 and 100 km" });
            }

            var studios = await _studioService.GetNearbyStudiosAsync(lat, lng, radius, cancellationToken);
            return Ok(studios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while searching nearby studios", error = ex.Message });
        }
    }

    
    
    
    [HttpGet("{id}/availability")]
    public async Task<IActionResult> GetStudioAvailability(
        int id,
        [FromQuery] DateTime date,
        CancellationToken cancellationToken)
    {
        try
        {
            var availability = await _studioService.GetStudioAvailabilityAsync(id, date, cancellationToken);
            return Ok(availability);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving studio availability", error = ex.Message });
        }
    }
} 