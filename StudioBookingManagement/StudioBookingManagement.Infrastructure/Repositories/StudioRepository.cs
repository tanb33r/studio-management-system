using Microsoft.EntityFrameworkCore;
using StudioBookingManagement.Domain.Entities;
using StudioBookingManagement.Domain.Repositories;
using StudioBookingManagement.Infrastructure.Data;

namespace StudioBookingManagement.Infrastructure.Repositories;

public class StudioRepository : BaseRepository<Studio>, IStudioRepository
{
    public StudioRepository(StudioBookingManagementDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Studio>> GetByAreaAsync(string area, CancellationToken cancellationToken = default)
    {
        return await _context.Studios
            .Where(s => s.IsActive && s.Area.ToLower().Contains(area.ToLower()))
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Studio>> GetActiveStudiosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Studios
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Studio>> GetNearbyStudiosAsync(decimal latitude, decimal longitude, double radiusKm, CancellationToken cancellationToken = default)
    {
        var allStudios = await _context.Studios
            .Where(s => s.IsActive)
            .ToListAsync(cancellationToken);

        return allStudios
            .Where(s => s.IsWithinRadius(latitude, longitude, radiusKm))
            .OrderBy(s => CalculateDistance(latitude, longitude, s.Latitude, s.Longitude));
    }

    public async Task<Studio?> GetStudioWithBookingsAsync(int studioId, CancellationToken cancellationToken = default)
    {
        return await _context.Studios
            .Include(s => s.Bookings)
            .FirstOrDefaultAsync(s => s.Id == studioId, cancellationToken);
    }

    public async Task<bool> IsStudioAvailableAsync(int studioId, DateTime date, TimeSpan startTime, TimeSpan endTime, CancellationToken cancellationToken = default)
    {
        var conflictingBookings = await _context.Bookings
            .Where(b => b.StudioId == studioId &&
                       b.Date.Date == date.Date &&
                       b.Status != BookingStatus.Cancelled &&
                       b.StartTime < endTime &&
                       b.EndTime > startTime)
            .AnyAsync(cancellationToken);

        return !conflictingBookings;
    }

    private static double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        const double EarthRadiusKm = 6371.0;
        
        var lat1Rad = (double)lat1 * Math.PI / 180.0;
        var lat2Rad = (double)lat2 * Math.PI / 180.0;
        var deltaLatRad = ((double)lat2 - (double)lat1) * Math.PI / 180.0;
        var deltaLonRad = ((double)lon2 - (double)lon1) * Math.PI / 180.0;

        var a = Math.Sin(deltaLatRad / 2) * Math.Sin(deltaLatRad / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(deltaLonRad / 2) * Math.Sin(deltaLonRad / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c;
    }
} 