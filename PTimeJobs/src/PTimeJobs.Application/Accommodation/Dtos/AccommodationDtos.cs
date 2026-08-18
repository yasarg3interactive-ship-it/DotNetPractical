namespace PTimeJobs.Application.Accommodation.Dtos;

public sealed record AccommodationProviderResponse(
    Guid AccommodationProviderId,
    Guid UserId,
    string BusinessName,
    string VerificationStatus,
    string? ContactNumber,
    DateTimeOffset CreatedAt);

public sealed record CreateAccommodationProviderRequest(Guid UserId, string BusinessName, string? ContactNumber);

public sealed record FacilityResponse(Guid FacilityId, string FacilityName, string? FacilityCategory);

public sealed record CreateFacilityRequest(string FacilityName, string? FacilityCategory);

public sealed record RoomTypeResponse(Guid RoomTypeId, string TypeName, string? Description);

public sealed record CreateRoomTypeRequest(string TypeName, string? Description);

public sealed record PropertyImageResponse(Guid PropertyImageId, string ImageUrl, int SortOrder, bool IsPrimary);

public sealed record AddPropertyImageRequest(string ImageUrl, int SortOrder, bool IsPrimary);

public sealed record PropertyFacilityResponse(Guid FacilityId, string FacilityName, string? Details);

public sealed record AddPropertyFacilityRequest(Guid FacilityId, string? Details);

public sealed record PropertyResponse(
    Guid PropertyId,
    Guid AccommodationProviderId,
    string PropertyName,
    string PropertyType,
    string? Description,
    Guid? LocationId,
    decimal? Latitude,
    decimal? Longitude,
    string? AddressText,
    bool IsActive,
    decimal AverageRating,
    int RatingCount,
    IReadOnlyCollection<PropertyImageResponse> Images,
    IReadOnlyCollection<PropertyFacilityResponse> Facilities);

public sealed record CreatePropertyRequest(
    Guid AccommodationProviderId,
    string PropertyName,
    string PropertyType,
    Guid? LocationId,
    decimal? Latitude,
    decimal? Longitude,
    string? AddressText,
    string? Description);

public sealed record RoomResponse(
    Guid RoomId,
    Guid PropertyId,
    Guid? RoomTypeId,
    string? RoomNumber,
    string? FloorNumber,
    int Capacity,
    int OccupiedCount,
    decimal MonthlyPrice,
    decimal? SecurityDeposit,
    bool IsAvailable);

public sealed record CreateRoomRequest(
    Guid PropertyId,
    int Capacity,
    decimal MonthlyPrice,
    Guid? RoomTypeId,
    string? RoomNumber,
    string? FloorNumber,
    decimal? SecurityDeposit);

public sealed record RoomAvailabilityResponse(Guid RoomAvailabilityId, Guid RoomId, DateOnly AvailableFrom, DateOnly? AvailableTo, int AvailableBeds, decimal? PriceOverride);

public sealed record AddRoomAvailabilityRequest(DateOnly AvailableFrom, int AvailableBeds, DateOnly? AvailableTo, decimal? PriceOverride);

public sealed record AccommodationBookingResponse(
    Guid BookingId,
    Guid RoomId,
    Guid WorkerProfileId,
    string Status,
    DateOnly CheckInDate,
    DateOnly? CheckOutDate,
    decimal? TotalAmount,
    DateTimeOffset CreatedAt);

public sealed record CreateAccommodationBookingRequest(Guid RoomId, Guid WorkerProfileId, DateOnly CheckInDate, decimal? TotalAmount);

public sealed record CompleteBookingRequest(DateOnly CheckOutDate);
