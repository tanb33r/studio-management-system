namespace StudioBookingManagement.Application.DTOs.Studio;

public class StudioDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal PricePerHour { get; set; }
    public string Currency { get; set; } = "BDT";
    public int Capacity { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string StudioType { get; set; } = string.Empty;
    public List<string> Amenities { get; set; } = new();
    public List<string> Equipment { get; set; } = new();
    public List<string> Images { get; set; } = new();
    public string OpeningHours { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class StudioSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public decimal PricePerHour { get; set; }
    public string Currency { get; set; } = "BDT";
    public string StudioType { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public List<string> Images { get; set; } = new();
    public double? DistanceKm { get; set; }
}

public class CreateStudioRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal PricePerHour { get; set; }
    public int Capacity { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string StudioType { get; set; } = string.Empty;
    public List<string> Amenities { get; set; } = new();
    public List<string> Equipment { get; set; } = new();
    public List<string> Images { get; set; } = new();
    public string OpeningHours { get; set; } = string.Empty;
}

public class UpdateStudioRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public decimal PricePerHour { get; set; }
    public int Capacity { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string StudioType { get; set; } = string.Empty;
    public List<string> Amenities { get; set; } = new();
    public List<string> Equipment { get; set; } = new();
    public List<string> Images { get; set; } = new();
    public string OpeningHours { get; set; } = string.Empty;
}

public class StudioAvailabilityDto
{
    public int StudioId { get; set; }
    public string StudioName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<TimeSlotDto> AvailableSlots { get; set; } = new();
    public List<TimeSlotDto> BookedSlots { get; set; } = new();
}

public class TimeSlotDto
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; }
    public string? BookingReference { get; set; }
} 