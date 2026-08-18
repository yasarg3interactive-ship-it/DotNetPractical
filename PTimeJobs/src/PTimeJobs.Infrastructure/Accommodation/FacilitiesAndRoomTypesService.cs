using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Domain.Accommodation;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Accommodation;

public sealed class FacilitiesAndRoomTypesService(ApplicationDbContext dbContext) : IFacilitiesAndRoomTypesService
{
    public async Task<IReadOnlyCollection<FacilityResponse>> GetFacilitiesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Facilities
            .AsNoTracking()
            .OrderBy(facility => facility.FacilityName)
            .Select(facility => new FacilityResponse(facility.FacilityId, facility.FacilityName, facility.FacilityCategory))
            .ToListAsync(cancellationToken);
    }

    public async Task<FacilityResponse> CreateFacilityAsync(CreateFacilityRequest request, CancellationToken cancellationToken = default)
    {
        var duplicate = await dbContext.Facilities
            .AsNoTracking()
            .AnyAsync(facility => facility.FacilityName == request.FacilityName, cancellationToken);

        if (duplicate)
        {
            throw new InvalidOperationException("A facility with this name already exists.");
        }

        var facility = Facility.Create(request.FacilityName, request.FacilityCategory);
        dbContext.Facilities.Add(facility);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new FacilityResponse(facility.FacilityId, facility.FacilityName, facility.FacilityCategory);
    }

    public async Task<IReadOnlyCollection<RoomTypeResponse>> GetRoomTypesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.RoomTypes
            .AsNoTracking()
            .OrderBy(roomType => roomType.TypeName)
            .Select(roomType => new RoomTypeResponse(roomType.RoomTypeId, roomType.TypeName, roomType.Description))
            .ToListAsync(cancellationToken);
    }

    public async Task<RoomTypeResponse> CreateRoomTypeAsync(CreateRoomTypeRequest request, CancellationToken cancellationToken = default)
    {
        var duplicate = await dbContext.RoomTypes
            .AsNoTracking()
            .AnyAsync(roomType => roomType.TypeName == request.TypeName, cancellationToken);

        if (duplicate)
        {
            throw new InvalidOperationException("A room type with this name already exists.");
        }

        var roomType = RoomType.Create(request.TypeName, request.Description);
        dbContext.RoomTypes.Add(roomType);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RoomTypeResponse(roomType.RoomTypeId, roomType.TypeName, roomType.Description);
    }
}
