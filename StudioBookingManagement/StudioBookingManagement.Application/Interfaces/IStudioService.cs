using StudioBookingManagement.Application.DTOs.Studio;

namespace StudioBookingManagement.Application.Interfaces;

public interface IStudioService
{
    Task<IEnumerable<StudioSummaryDto>> GetAllStudiosAsync(CancellationToken cancellationToken = default);
    Task<StudioDto?> GetStudioByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudioSummaryDto>> GetStudiosByAreaAsync(string area, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudioSummaryDto>> GetNearbyStudiosAsync(decimal latitude, decimal longitude, double radiusKm, CancellationToken cancellationToken = default);
    Task<StudioAvailabilityDto> GetStudioAvailabilityAsync(int studioId, DateTime date, CancellationToken cancellationToken = default);
} 