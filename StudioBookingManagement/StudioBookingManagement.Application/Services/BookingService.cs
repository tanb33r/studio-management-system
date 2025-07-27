using StudioBookingManagement.Application.DTOs.Booking;
using StudioBookingManagement.Application.Interfaces;
using StudioBookingManagement.Domain.Entities;
using StudioBookingManagement.Domain.Repositories;

namespace StudioBookingManagement.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IStudioRepository _studioRepository;

    public BookingService(IBookingRepository bookingRepository, IStudioRepository studioRepository)
    {
        _bookingRepository = bookingRepository;
        _studioRepository = studioRepository;
    }

    public async Task<IEnumerable<BookingSummaryDto>> GetAllBookingsAsync(CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetAllAsync(cancellationToken);
        return bookings.Select(MapToSummaryDto);
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);
        return booking != null ? MapToDto(booking) : null;
    }

    public async Task<BookingDto?> GetBookingByReferenceAsync(string bookingReference, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetBookingByReferenceAsync(bookingReference, cancellationToken);
        return booking != null ? MapToDto(booking) : null;
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(string email, CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetBookingsByUserAsync(email, cancellationToken);
        return bookings.Select(MapToDto);
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByStudioAsync(int studioId, CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetBookingsByStudioAsync(studioId, cancellationToken);
        return bookings.Select(MapToDto);
    }

    public async Task<BookingConflictResponseDto> CheckBookingAvailabilityAsync(BookingAvailabilityRequestDto request, CancellationToken cancellationToken = default)
    {
        var studio = await _studioRepository.GetByIdAsync(request.StudioId, cancellationToken);
        if (studio == null)
        {
            return new BookingConflictResponseDto
            {
                HasConflict = true,
                Message = "Studio not found"
            };
        }

        if (!studio.IsActive)
        {
            return new BookingConflictResponseDto
            {
                HasConflict = true,
                Message = "Studio is not active"
            };
        }

        if (request.StartTime >= request.EndTime)
        {
            return new BookingConflictResponseDto
            {
                HasConflict = true,
                Message = "End time must be after start time"
            };
        }

        if (request.Date.Date < DateTime.UtcNow.Date)
        {
            return new BookingConflictResponseDto
            {
                HasConflict = true,
                Message = "Cannot book for past dates"
            };
        }

        var conflictingBookings = await _bookingRepository.GetConflictingBookingsAsync(
            request.StudioId, request.Date, request.StartTime, request.EndTime, cancellationToken);

        if (conflictingBookings.Any())
        {
            return new BookingConflictResponseDto
            {
                HasConflict = true,
                Message = "Time slot is already booked",
                ConflictingBookings = conflictingBookings.Select(MapToConflictingDto).ToList()
            };
        }

        return new BookingConflictResponseDto
        {
            HasConflict = false,
            Message = "Time slot is available"
        };
    }

    public async Task<BookingDto> CreateBookingAsync(CreateBookingRequestDto request, CancellationToken cancellationToken = default)
    {
        var availabilityCheck = await CheckBookingAvailabilityAsync(new BookingAvailabilityRequestDto
        {
            StudioId = request.StudioId,
            Date = request.Date,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        }, cancellationToken);

        if (availabilityCheck.HasConflict)
        {
            throw new InvalidOperationException($"Booking conflict: {availabilityCheck.Message}");
        }

        var studio = await _studioRepository.GetByIdAsync(request.StudioId, cancellationToken);
        if (studio == null)
            throw new ArgumentException($"Studio with ID {request.StudioId} not found");

        var durationHours = (int)Math.Ceiling((request.EndTime - request.StartTime).TotalHours);
        var totalPrice = studio.PricePerHour * durationHours;

        var booking = new Booking(
            request.StudioId,
            request.UserName,
            request.Email,
            request.Phone,
            request.Date,
            request.StartTime,
            request.EndTime,
            totalPrice,
            request.Notes);

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        var createdBooking = await _bookingRepository.GetByIdAsync(booking.Id, cancellationToken);
        return MapToDto(createdBooking!);
    }

    public async Task<BookingDto> UpdateBookingAsync(int id, UpdateBookingRequestDto request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);
        if (booking == null)
            throw new ArgumentException($"Booking with ID {id} not found");

        if (booking.Status != BookingStatus.Pending)
            throw new InvalidOperationException("Only pending bookings can be updated");

        var hasConflicts = await _bookingRepository.HasConflictingBookingsAsync(
            booking.StudioId, request.Date, request.StartTime, request.EndTime, id, cancellationToken);

        if (hasConflicts)
        {
            throw new InvalidOperationException("Updated time slot conflicts with existing booking");
        }

        var studio = booking.Studio;
        var durationHours = (int)Math.Ceiling((request.EndTime - request.StartTime).TotalHours);
        var totalPrice = studio.PricePerHour * durationHours;

        booking.UpdateBookingDetails(request.Date, request.StartTime, request.EndTime, totalPrice, request.Notes);

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(booking);
    }

    public async Task<BookingConfirmationDto> ConfirmBookingAsync(int id, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);
        if (booking == null)
            throw new ArgumentException($"Booking with ID {id} not found");

        var hasConflicts = await _bookingRepository.HasConflictingBookingsAsync(
            booking.StudioId, booking.Date, booking.StartTime, booking.EndTime, id, cancellationToken);

        if (hasConflicts)
        {
            return new BookingConfirmationDto
            {
                BookingId = id,
                BookingReference = booking.BookingReference,
                IsConfirmed = false,
                Message = "Cannot confirm booking due to conflicting reservations"
            };
        }

        booking.Confirm();
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        return new BookingConfirmationDto
        {
            BookingId = id,
            BookingReference = booking.BookingReference,
            IsConfirmed = true,
            Message = "Booking confirmed successfully",
            ConfirmedAt = booking.ConfirmedAt
        };
    }

    public async Task<BookingDto> CancelBookingAsync(int id, string reason = "", CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);
        if (booking == null)
            throw new ArgumentException($"Booking with ID {id} not found");

        if (!booking.CanBeCancelled())
            throw new InvalidOperationException("Booking cannot be cancelled (less than 24 hours before start time or already completed)");

        booking.Cancel(reason);
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(booking);
    }

    public async Task<bool> DeleteBookingAsync(int id, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);
        if (booking == null)
            return false;

        if (booking.Status != BookingStatus.Cancelled && 
            !(booking.Status == BookingStatus.Completed && booking.Date < DateTime.UtcNow.AddDays(-90)))
        {
            throw new InvalidOperationException("Only cancelled bookings or old completed bookings can be deleted");
        }

        await _bookingRepository.DeleteAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static BookingDto MapToDto(Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            StudioId = booking.StudioId,
            StudioName = booking.Studio?.Name ?? string.Empty,
            UserName = booking.UserName,
            Email = booking.Email,
            Phone = booking.Phone,
            Date = booking.Date,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            DurationHours = booking.DurationHours,
            TotalPrice = booking.TotalPrice,
            Currency = booking.Currency,
            Status = booking.Status,
            Notes = booking.Notes,
            BookingReference = booking.BookingReference,
            CreatedAt = booking.CreatedAt,
            ConfirmedAt = booking.ConfirmedAt,
            CancelledAt = booking.CancelledAt,
            CancellationReason = booking.CancellationReason
        };
    }

    private static BookingSummaryDto MapToSummaryDto(Booking booking)
    {
        return new BookingSummaryDto
        {
            Id = booking.Id,
            StudioName = booking.Studio?.Name ?? string.Empty,
            UserName = booking.UserName,
            Email = booking.Email,
            Date = booking.Date,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            BookingReference = booking.BookingReference,
            TotalPrice = booking.TotalPrice,
            Currency = booking.Currency
        };
    }

    private static ConflictingBookingDto MapToConflictingDto(Booking booking)
    {
        return new ConflictingBookingDto
        {
            Id = booking.Id,
            Date = booking.Date,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            BookingReference = booking.BookingReference,
            Status = booking.Status
        };
    }
} 