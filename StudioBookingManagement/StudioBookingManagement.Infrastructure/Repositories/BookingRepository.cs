using Microsoft.EntityFrameworkCore;
using StudioBookingManagement.Domain.Entities;
using StudioBookingManagement.Domain.Repositories;
using StudioBookingManagement.Infrastructure.Data;

namespace StudioBookingManagement.Infrastructure.Repositories;

public class BookingRepository : BaseRepository<Booking>, IBookingRepository
{
    public BookingRepository(StudioBookingManagementDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Booking>> GetBookingsByStudioAsync(int studioId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Studio)
            .Where(b => b.StudioId == studioId)
            .OrderByDescending(b => b.Date)
            .ThenBy(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByStudioAndDateAsync(int studioId, DateTime date, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Studio)
            .Where(b => b.StudioId == studioId && b.Date.Date == date.Date)
            .OrderBy(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetConflictingBookingsAsync(int studioId, DateTime date, TimeSpan startTime, TimeSpan endTime, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Studio)
            .Where(b => b.StudioId == studioId &&
                       b.Date.Date == date.Date &&
                       b.Status != BookingStatus.Cancelled &&
                       b.StartTime < endTime &&
                       b.EndTime > startTime)
            .OrderBy(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<Booking?> GetBookingByReferenceAsync(string bookingReference, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Studio)
            .FirstOrDefaultAsync(b => b.BookingReference == bookingReference, cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Studio)
            .Where(b => b.Email.ToLower() == email.ToLower())
            .OrderByDescending(b => b.Date)
            .ThenBy(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(CancellationToken cancellationToken = default)
    {
        var currentDateTime = DateTime.UtcNow;
        
        return await _context.Bookings
            .Include(b => b.Studio)
            .Where(b => b.Status == BookingStatus.Confirmed &&
                       (b.Date.Date > currentDateTime.Date ||
                        (b.Date.Date == currentDateTime.Date && b.StartTime > currentDateTime.TimeOfDay)))
            .OrderBy(b => b.Date)
            .ThenBy(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Studio)
            .Where(b => b.Date.Date >= startDate.Date && b.Date.Date <= endDate.Date)
            .OrderBy(b => b.Date)
            .ThenBy(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasConflictingBookingsAsync(int studioId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeBookingId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Bookings
            .Where(b => b.StudioId == studioId &&
                       b.Date.Date == date.Date &&
                       b.Status != BookingStatus.Cancelled &&
                       b.StartTime < endTime &&
                       b.EndTime > startTime);

        if (excludeBookingId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBookingId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public override async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Studio)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Studio)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }
} 