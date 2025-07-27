using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudioBookingManagement.Application.DTOs.Booking;
using StudioBookingManagement.Application.Interfaces;

namespace StudioBookingManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBookings(CancellationToken cancellationToken)
    {
        try
        {
            var bookings = await _bookingService.GetAllBookingsAsync(cancellationToken);
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving bookings", error = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(int id, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(id, cancellationToken);
            if (booking == null)
            {
                return NotFound(new { message = $"Booking with ID {id} not found" });
            }

            return Ok(booking);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the booking", error = ex.Message });
        }
    }

    [HttpGet("reference/{bookingReference}")]
    public async Task<IActionResult> GetBookingByReference(string bookingReference, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _bookingService.GetBookingByReferenceAsync(bookingReference, cancellationToken);
            if (booking == null)
            {
                return NotFound(new { message = $"Booking with reference {bookingReference} not found" });
            }

            return Ok(booking);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the booking", error = ex.Message });
        }
    }
    
    [HttpGet("user/{email}")]
    public async Task<IActionResult> GetBookingsByUser(string email, CancellationToken cancellationToken)
    {
        try
        {
            var bookings = await _bookingService.GetBookingsByUserAsync(email, cancellationToken);
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving user bookings", error = ex.Message });
        }
    }

    [HttpGet("studio/{studioId}")]
    public async Task<IActionResult> GetBookingsByStudio(int studioId, CancellationToken cancellationToken)
    {
        try
        {
            var bookings = await _bookingService.GetBookingsByStudioAsync(studioId, cancellationToken);
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving studio bookings", error = ex.Message });
        }
    }

    [HttpPost("check-availability")]
    public async Task<IActionResult> CheckAvailability([FromBody] BookingAvailabilityRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _bookingService.CheckBookingAvailabilityAsync(request, cancellationToken);
            
            if (result.HasConflict)
            {
                return Conflict(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while checking availability", error = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _bookingService.CreateBookingAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = "Booking already exists" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the booking", error = ex.Message });
        }
    }

    
    
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _bookingService.UpdateBookingAsync(id, request, cancellationToken);
            return Ok(booking);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the booking", error = ex.Message });
        }
    }

    
    
    
    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> ConfirmBooking(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _bookingService.ConfirmBookingAsync(id, cancellationToken);
            
            if (!result.IsConfirmed)
            {
                return Conflict(result);
            }

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while confirming the booking", error = ex.Message });
        }
    }

    
    
    
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id, [FromBody] CancelBookingRequestDto? request, CancellationToken cancellationToken)
    {
        try
        {
            var reason = request?.Reason ?? string.Empty;
            var booking = await _bookingService.CancelBookingAsync(id, reason, cancellationToken);
            return Ok(booking);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while cancelling the booking", error = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteBooking(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _bookingService.DeleteBookingAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound(new { message = $"Booking with ID {id} not found" });
            }

            return Ok(new { message = "Booking deleted successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the booking", error = ex.Message });
        }
    }
}

public class CancelBookingRequestDto
{
    public string Reason { get; set; } = string.Empty;
} 