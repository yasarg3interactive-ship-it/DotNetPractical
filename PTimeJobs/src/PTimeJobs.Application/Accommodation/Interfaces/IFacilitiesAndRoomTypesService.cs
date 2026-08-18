using PTimeJobs.Application.Accommodation.Dtos;

namespace PTimeJobs.Application.Accommodation.Interfaces;

public interface IFacilitiesAndRoomTypesService
{
    Task<IReadOnlyCollection<FacilityResponse>> GetFacilitiesAsync(CancellationToken cancellationToken = default);

    Task<FacilityResponse> CreateFacilityAsync(CreateFacilityRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<RoomTypeResponse>> GetRoomTypesAsync(CancellationToken cancellationToken = default);

    Task<RoomTypeResponse> CreateRoomTypeAsync(CreateRoomTypeRequest request, CancellationToken cancellationToken = default);
}
