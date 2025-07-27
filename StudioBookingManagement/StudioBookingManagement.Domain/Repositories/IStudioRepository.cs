using StudioBookingManagement.Domain.Common;
using StudioBookingManagement.Domain.Entities;

namespace StudioBookingManagement.Domain.Repositories;

public interface IStudioRepository : IRepository<Studio>
{
    Task<IEnumerable<Studio>> GetByAreaAsync(string area, CancellationToken cancellationToken = default);
    Task<IEnumerable<Studio>> GetActiveStudiosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Studio>> GetNearbyStudiosAsync(decimal latitude, decimal longitude, double radiusKm, CancellationToken cancellationToken = default);
    Task<Studio?> GetStudioWithBookingsAsync(int studioId, CancellationToken cancellationToken = default);
    Task<bool> IsStudioAvailableAsync(int studioId, DateTime date, TimeSpan startTime, TimeSpan endTime, CancellationToken cancellationToken = default);
} 