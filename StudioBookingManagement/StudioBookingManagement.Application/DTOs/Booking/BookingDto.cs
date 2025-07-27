using StudioBookingManagement.Domain.Entities;

namespace StudioBookingManagement.Application.DTOs.Booking;

public class BookingDto
{
    public int Id { get; set; }
    public int StudioId { get; set; }
    public string StudioName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int DurationHours { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = "BDT";
    public BookingStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string BookingReference { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
}

public class BookingSummaryDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string StudioName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public BookingStatus Status { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = "BDT";
}

public class CreateBookingRequestDto
{
    public int StudioId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class UpdateBookingRequestDto
{
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class BookingConflictResponseDto
{
    public bool HasConflict { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<ConflictingBookingDto> ConflictingBookings { get; set; } = new();
}

public class ConflictingBookingDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public BookingStatus Status { get; set; }
}

public class BookingAvailabilityRequestDto
{
    public int StudioId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class BookingConfirmationDto
{
    public int BookingId { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public bool IsConfirmed { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? ConfirmedAt { get; set; }
} 