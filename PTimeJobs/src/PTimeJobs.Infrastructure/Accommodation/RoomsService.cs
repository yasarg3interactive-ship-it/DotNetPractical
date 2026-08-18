using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Domain.Accommodation;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Accommodation;

public sealed class RoomsService(ApplicationDbContext dbContext) : IRoomsService
{
    public async Task<RoomResponse?> GetByIdAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        var room = await dbContext.Rooms.AsNoTracking().FirstOrDefaultAsync(r => r.RoomId == roomId, cancellationToken);
        return room is null ? null : ToResponse(room);
    }

    public async Task<IReadOnlyCollection<RoomResponse>> GetByPropertyAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        var rooms = await dbContext.Rooms.AsNoTracking().Where(r => r.PropertyId == propertyId).ToListAsync(cancellationToken);
        return rooms.Select(ToResponse).ToList();
    }

    public async Task<RoomResponse> CreateAsync(CreateRoomRequest request, CancellationToken cancellationToken = default)
    {
        var propertyExists = await dbContext.Properties.AsNoTracking().AnyAsync(p => p.PropertyId == request.PropertyId, cancellationToken);
        if (!propertyExists)
        {
            throw new InvalidOperationException("Property not found.");
        }

        var room = Room.Create(
            request.PropertyId,
            request.Capacity,
            request.MonthlyPrice,
            request.RoomTypeId,
            request.RoomNumber,
            request.FloorNumber,
            request.SecurityDeposit);

        dbContext.Rooms.Add(room);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(room);
    }

    public async Task<IReadOnlyCollection<RoomAvailabilityResponse>> GetAvailabilityAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        return await dbContext.RoomAvailabilities
            .AsNoTracking()
            .Where(availability => availability.RoomId == roomId)
            .OrderBy(availability => availability.AvailableFrom)
            .Select(availability => new RoomAvailabilityResponse(
                availability.RoomAvailabilityId,
                availability.RoomId,
                availability.AvailableFrom,
                availability.AvailableTo,
                availability.AvailableBeds,
                availability.PriceOverride))
            .ToListAsync(cancellationToken);
    }

    public async Task<RoomAvailabilityResponse?> AddAvailabilityAsync(
        Guid roomId,
        AddRoomAvailabilityRequest request,
        CancellationToken cancellationToken = default)
    {
        var roomExists = await dbContext.Rooms.AsNoTracking().AnyAsync(r => r.RoomId == roomId, cancellationToken);
        if (!roomExists)
        {
            return null;
        }

        var availability = RoomAvailability.Create(roomId, request.AvailableFrom, request.AvailableBeds, request.AvailableTo, request.PriceOverride);
        dbContext.RoomAvailabilities.Add(availability);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RoomAvailabilityResponse(
            availability.RoomAvailabilityId,
            availability.RoomId,
            availability.AvailableFrom,
            availability.AvailableTo,
            availability.AvailableBeds,
            availability.PriceOverride);
    }

    private static RoomResponse ToResponse(Room room) => new(
        room.RoomId,
        room.PropertyId,
        room.RoomTypeId,
        room.RoomNumber,
        room.FloorNumber,
        room.Capacity,
        room.OccupiedCount,
        room.MonthlyPrice,
        room.SecurityDeposit,
        room.IsAvailable);
}
