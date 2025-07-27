using StudioBookingManagement.Application.DTOs.Booking;

namespace StudioBookingManagement.Application.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<BookingSummaryDto>> GetAllBookingsAsync(CancellationToken cancellationToken = default);
    Task<BookingDto?> GetBookingByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BookingDto?> GetBookingByReferenceAsync(string bookingReference, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> GetBookingsByStudioAsync(int studioId, CancellationToken cancellationToken = default);
    Task<BookingConflictResponseDto> CheckBookingAvailabilityAsync(BookingAvailabilityRequestDto request, CancellationToken cancellationToken = default);
    Task<BookingDto> CreateBookingAsync(CreateBookingRequestDto request, CancellationToken cancellationToken = default);
    Task<BookingDto> UpdateBookingAsync(int id, UpdateBookingRequestDto request, CancellationToken cancellationToken = default);
    Task<BookingConfirmationDto> ConfirmBookingAsync(int id, CancellationToken cancellationToken = default);
    Task<BookingDto> CancelBookingAsync(int id, string reason = "", CancellationToken cancellationToken = default);
    Task<bool> DeleteBookingAsync(int id, CancellationToken cancellationToken = default);
} 