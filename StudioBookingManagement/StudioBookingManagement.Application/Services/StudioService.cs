using StudioBookingManagement.Application.DTOs.Studio;
using StudioBookingManagement.Application.Interfaces;
using StudioBookingManagement.Domain.Entities;
using StudioBookingManagement.Domain.Repositories;

namespace StudioBookingManagement.Application.Services;

public class StudioService : IStudioService
{
    private readonly IStudioRepository _studioRepository;
    private readonly IBookingRepository _bookingRepository;

    public StudioService(IStudioRepository studioRepository, IBookingRepository bookingRepository)
    {
        _studioRepository = studioRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<StudioSummaryDto>> GetAllStudiosAsync(CancellationToken cancellationToken = default)
    {
        var studios = await _studioRepository.GetActiveStudiosAsync(cancellationToken);
        return studios.Select(MapToSummaryDto);
    }

    public async Task<StudioDto?> GetStudioByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var studio = await _studioRepository.GetByIdAsync(id, cancellationToken);
        return studio != null ? MapToDto(studio) : null;
    }

    public async Task<IEnumerable<StudioSummaryDto>> GetStudiosByAreaAsync(string area, CancellationToken cancellationToken = default)
    {
        var studios = await _studioRepository.GetByAreaAsync(area, cancellationToken);
        return studios.Select(MapToSummaryDto);
    }

    public async Task<IEnumerable<StudioSummaryDto>> GetNearbyStudiosAsync(decimal latitude, decimal longitude, double radiusKm, CancellationToken cancellationToken = default)
    {
        var studios = await _studioRepository.GetNearbyStudiosAsync(latitude, longitude, radiusKm, cancellationToken);
        return studios.Select(studio => 
        {
            var dto = MapToSummaryDto(studio);
            dto.DistanceKm = CalculateDistance(latitude, longitude, studio.Latitude, studio.Longitude);
            return dto;
        });
    }

    public async Task<StudioAvailabilityDto> GetStudioAvailabilityAsync(int studioId, DateTime date, CancellationToken cancellationToken = default)
    {
        var studio = await _studioRepository.GetByIdAsync(studioId, cancellationToken);
        if (studio == null)
            throw new ArgumentException($"Studio with ID {studioId} not found");

        var bookings = await _bookingRepository.GetBookingsByStudioAndDateAsync(studioId, date, cancellationToken);
        var bookedSlots = bookings
            .Where(b => b.Status != BookingStatus.Cancelled)
            .Select(b => new TimeSlotDto
            {
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                IsAvailable = false,
                BookingReference = b.BookingReference
            })
            .ToList();

        var availableSlots = new List<TimeSlotDto>();
        for (int hour = 9; hour < 21; hour++)
        {
            var startTime = new TimeSpan(hour, 0, 0);
            var endTime = new TimeSpan(hour + 1, 0, 0);
            
            var isBooked = bookedSlots.Any(b => 
                b.StartTime < endTime && b.EndTime > startTime);

            if (!isBooked)
            {
                availableSlots.Add(new TimeSlotDto
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    IsAvailable = true
                });
            }
        }

        return new StudioAvailabilityDto
        {
            StudioId = studioId,
            StudioName = studio.Name,
            Date = date,
            AvailableSlots = availableSlots,
            BookedSlots = bookedSlots
        };
    }

    private static StudioDto MapToDto(Studio studio)
    {
        return new StudioDto
        {
            Id = studio.Id,
            Name = studio.Name,
            Description = studio.Description,
            Area = studio.Area,
            Address = studio.Address,
            City = studio.City,
            State = studio.State,
            ZipCode = studio.ZipCode,
            Country = studio.Country,
            Latitude = studio.Latitude,
            Longitude = studio.Longitude,
            PricePerHour = studio.PricePerHour,
            Currency = studio.Currency,
            Capacity = studio.Capacity,
            ContactPhone = studio.ContactPhone,
            ContactEmail = studio.ContactEmail,
            StudioType = studio.StudioType,
            Amenities = studio.Amenities,
            Equipment = studio.Equipment,
            Images = studio.Images,
            OpeningHours = studio.OpeningHours,
            IsActive = studio.IsActive,
            Rating = studio.Rating,
            ReviewCount = studio.ReviewCount,
            CreatedAt = studio.CreatedAt,
            UpdatedAt = studio.UpdatedAt
        };
    }

    private static StudioSummaryDto MapToSummaryDto(Studio studio)
    {
        return new StudioSummaryDto
        {
            Id = studio.Id,
            Name = studio.Name,
            Area = studio.Area,
            City = studio.City,
            State = studio.State,
            PricePerHour = studio.PricePerHour,
            Currency = studio.Currency,
            StudioType = studio.StudioType,
            Rating = studio.Rating,
            ReviewCount = studio.ReviewCount,
            Images = studio.Images
        };
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