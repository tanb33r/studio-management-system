using StudioBookingManagement.Domain.Common;

namespace StudioBookingManagement.Domain.Entities;

public class Studio : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Area { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public decimal PricePerHour { get; private set; }
    public string Currency { get; private set; } = "BDT";
    public int Capacity { get; private set; }
    public string ContactPhone { get; private set; } = string.Empty;
    public string ContactEmail { get; private set; } = string.Empty;
    public string StudioType { get; private set; } = string.Empty; 
    public List<string> Amenities { get; private set; } = new();
    public List<string> Equipment { get; private set; } = new();
    public List<string> Images { get; private set; } = new();
    public string OpeningHours { get; private set; } = string.Empty; 
    public bool IsActive { get; private set; } = true;
    public string OwnerId { get; private set; } = string.Empty;
    public double Rating { get; private set; } = 0;
    public int ReviewCount { get; private set; } = 0;

    public virtual ICollection<Booking> Bookings { get; private set; } = new List<Booking>();

    private Studio() { }

    public Studio(
        string name, 
        string description, 
        string area, 
        string address,
        string city,
        string state,
        string zipCode,
        string country,
        decimal latitude,
        decimal longitude,
        decimal pricePerHour,
        int capacity,
        string contactPhone,
        string contactEmail,
        string studioType,
        string ownerId)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Area = area ?? throw new ArgumentNullException(nameof(area));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Latitude = latitude;
        Longitude = longitude;
        PricePerHour = pricePerHour;
        Capacity = capacity;
        ContactPhone = contactPhone ?? throw new ArgumentNullException(nameof(contactPhone));
        ContactEmail = contactEmail ?? throw new ArgumentNullException(nameof(contactEmail));
        StudioType = studioType ?? throw new ArgumentNullException(nameof(studioType));
        OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string name,
        string description,
        string area,
        string address,
        string city,
        string state,
        string zipCode,
        decimal pricePerHour,
        int capacity,
        string contactPhone,
        string contactEmail,
        string studioType)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Area = area ?? throw new ArgumentNullException(nameof(area));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        PricePerHour = pricePerHour;
        Capacity = capacity;
        ContactPhone = contactPhone ?? throw new ArgumentNullException(nameof(contactPhone));
        ContactEmail = contactEmail ?? throw new ArgumentNullException(nameof(contactEmail));
        StudioType = studioType ?? throw new ArgumentNullException(nameof(studioType));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLocation(decimal latitude, decimal longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAmenities(List<string> amenities)
    {
        Amenities = amenities ?? new List<string>();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEquipment(List<string> equipment)
    {
        Equipment = equipment ?? new List<string>();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateImages(List<string> images)
    {
        Images = images ?? new List<string>();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateOpeningHours(string openingHours)
    {
        OpeningHours = openingHours ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRating(double rating, int reviewCount)
    {
        Rating = rating;
        ReviewCount = reviewCount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsWithinRadius(decimal latitude, decimal longitude, double radiusKm)
    {
        const double EarthRadiusKm = 6371.0;
        
        var lat1Rad = (double)Latitude * Math.PI / 180.0;
        var lat2Rad = (double)latitude * Math.PI / 180.0;
        var deltaLatRad = ((double)latitude - (double)Latitude) * Math.PI / 180.0;
        var deltaLonRad = ((double)longitude - (double)Longitude) * Math.PI / 180.0;

        var a = Math.Sin(deltaLatRad / 2) * Math.Sin(deltaLatRad / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(deltaLonRad / 2) * Math.Sin(deltaLonRad / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = EarthRadiusKm * c;

        return distance <= radiusKm;
    }
} 