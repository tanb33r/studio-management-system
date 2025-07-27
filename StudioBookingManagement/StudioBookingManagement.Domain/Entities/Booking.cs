using StudioBookingManagement.Domain.Common;

namespace StudioBookingManagement.Domain.Entities;

public class Booking : BaseEntity
{
    public int StudioId { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public int DurationHours { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string Currency { get; private set; } = "BDT";
    public BookingStatus Status { get; private set; } = BookingStatus.Pending;
    public string Notes { get; private set; } = string.Empty;
    public string BookingReference { get; private set; } = string.Empty;
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }

    public virtual Studio Studio { get; private set; } = null!;

    private Booking() { }

    public Booking(
        int studioId,
        string userName,
        string email,
        string phone,
        DateTime date,
        TimeSpan startTime,
        TimeSpan endTime,
        decimal totalPrice,
        string notes = "")
    {
        StudioId = studioId;
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
        Date = date.Date; 
        StartTime = startTime;
        EndTime = endTime;
        DurationHours = CalculateDurationHours(startTime, endTime);
        TotalPrice = totalPrice;
        Notes = notes ?? string.Empty;
        Status = BookingStatus.Pending;
        BookingReference = GenerateBookingReference();
        CreatedAt = DateTime.UtcNow;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new InvalidOperationException("Only pending bookings can be confirmed");

        Status = BookingStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel(string reason = "")
    {
        if (Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Booking is already cancelled");

        if (Status == BookingStatus.Completed)
            throw new InvalidOperationException("Completed bookings cannot be cancelled");

        Status = BookingStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancellationReason = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed bookings can be completed");

        Status = BookingStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBookingDetails(
        DateTime date,
        TimeSpan startTime,
        TimeSpan endTime,
        decimal totalPrice,
        string notes = "")
    {
        if (Status != BookingStatus.Pending)
            throw new InvalidOperationException("Only pending bookings can be updated");

        Date = date.Date;
        StartTime = startTime;
        EndTime = endTime;
        DurationHours = CalculateDurationHours(startTime, endTime);
        TotalPrice = totalPrice;
        Notes = notes ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool ConflictsWith(DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        if (Status == BookingStatus.Cancelled)
            return false;

        if (Date.Date != date.Date)
            return false;

        return StartTime < endTime && EndTime > startTime;
    }

    public bool IsUpcoming()
    {
        var bookingDateTime = Date.Add(StartTime);
        return bookingDateTime > DateTime.UtcNow && Status == BookingStatus.Confirmed;
    }

    public bool CanBeCancelled()
    {
        var bookingDateTime = Date.Add(StartTime);
        var hoursUntilBooking = (bookingDateTime - DateTime.UtcNow).TotalHours;
        
        return hoursUntilBooking >= 24 && 
               (Status == BookingStatus.Pending || Status == BookingStatus.Confirmed);
    }

    private static int CalculateDurationHours(TimeSpan startTime, TimeSpan endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time");

        return (int)Math.Ceiling((endTime - startTime).TotalHours);
    }

    private static string GenerateBookingReference()
    {
        return $"BK{DateTime.UtcNow:yyyyMMdd}{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
    }
}

public enum BookingStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2,
    Completed = 3
} 