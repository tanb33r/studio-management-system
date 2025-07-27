using StudioBookingManagement.Domain.Common;
using StudioBookingManagement.Domain.Entities;

namespace StudioBookingManagement.Domain.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByStudioAsync(int studioId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetBookingsByStudioAndDateAsync(int studioId, DateTime date, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetConflictingBookingsAsync(int studioId, DateTime date, TimeSpan startTime, TimeSpan endTime, CancellationToken cancellationToken = default);
    Task<Booking?> GetBookingByReferenceAsync(string bookingReference, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetBookingsByUserAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<bool> HasConflictingBookingsAsync(int studioId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeBookingId = null, CancellationToken cancellationToken = default);
} 