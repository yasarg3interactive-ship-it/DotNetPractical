using PTimeJobs.Application.Accommodation.Dtos;

namespace PTimeJobs.Application.Accommodation.Interfaces;

public interface IRoomsService
{
    Task<RoomResponse?> GetByIdAsync(Guid roomId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<RoomResponse>> GetByPropertyAsync(Guid propertyId, CancellationToken cancellationToken = default);

    Task<RoomResponse> CreateAsync(CreateRoomRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<RoomAvailabilityResponse>> GetAvailabilityAsync(Guid roomId, CancellationToken cancellationToken = default);

    Task<RoomAvailabilityResponse?> AddAvailabilityAsync(Guid roomId, AddRoomAvailabilityRequest request, CancellationToken cancellationToken = default);
}
