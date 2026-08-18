using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Domain.Accommodation;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Accommodation;

public sealed class AccommodationBookingsService(ApplicationDbContext dbContext) : IAccommodationBookingsService
{
    public async Task<AccommodationBookingResponse?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await dbContext.AccommodationBookings.AsNoTracking().FirstOrDefaultAsync(b => b.BookingId == bookingId, cancellationToken);
        return booking is null ? null : ToResponse(booking);
    }

    public async Task<IReadOnlyCollection<AccommodationBookingResponse>> GetByWorkerAsync(Guid workerProfileId, CancellationToken cancellationToken = default)
    {
        var bookings = await dbContext.AccommodationBookings
            .AsNoTracking()
            .Where(b => b.WorkerProfileId == workerProfileId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);

        return bookings.Select(ToResponse).ToList();
    }

    public async Task<AccommodationBookingResponse> CreateAsync(CreateAccommodationBookingRequest request, CancellationToken cancellationToken = default)
    {
        var roomExists = await dbContext.Rooms.AsNoTracking().AnyAsync(room => room.RoomId == request.RoomId, cancellationToken);
        if (!roomExists)
        {
            throw new InvalidOperationException("Room not found.");
        }

        var workerExists = await dbContext.WorkerProfiles
            .AsNoTracking()
            .AnyAsync(worker => worker.WorkerProfileId == request.WorkerProfileId, cancellationToken);

        if (!workerExists)
        {
            throw new InvalidOperationException("Worker profile not found.");
        }

        var booking = AccommodationBooking.Create(request.RoomId, request.WorkerProfileId, request.CheckInDate, request.TotalAmount);
        dbContext.AccommodationBookings.Add(booking);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(booking);
    }

    public async Task<AccommodationBookingResponse?> ConfirmAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await dbContext.AccommodationBookings.FirstOrDefaultAsync(b => b.BookingId == bookingId, cancellationToken);
        if (booking is null)
        {
            return null;
        }

        booking.Confirm();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(booking);
    }

    public async Task<AccommodationBookingResponse?> CheckInAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await dbContext.AccommodationBookings.FirstOrDefaultAsync(b => b.BookingId == bookingId, cancellationToken);
        if (booking is null)
        {
            return null;
        }

        booking.CheckIn();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(booking);
    }

    public async Task<AccommodationBookingResponse?> CompleteAsync(Guid bookingId, CompleteBookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await dbContext.AccommodationBookings.FirstOrDefaultAsync(b => b.BookingId == bookingId, cancellationToken);
        if (booking is null)
        {
            return null;
        }

        booking.Complete(request.CheckOutDate);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(booking);
    }

    public async Task<AccommodationBookingResponse?> CancelAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await dbContext.AccommodationBookings.FirstOrDefaultAsync(b => b.BookingId == bookingId, cancellationToken);
        if (booking is null)
        {
            return null;
        }

        booking.Cancel();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(booking);
    }

    public async Task<AccommodationBookingResponse?> RejectAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await dbContext.AccommodationBookings.FirstOrDefaultAsync(b => b.BookingId == bookingId, cancellationToken);
        if (booking is null)
        {
            return null;
        }

        booking.Reject();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(booking);
    }

    private static AccommodationBookingResponse ToResponse(AccommodationBooking booking) => new(
        booking.BookingId,
        booking.RoomId,
        booking.WorkerProfileId,
        booking.Status.ToString(),
        booking.CheckInDate,
        booking.CheckOutDate,
        booking.TotalAmount,
        booking.CreatedAt);
}
