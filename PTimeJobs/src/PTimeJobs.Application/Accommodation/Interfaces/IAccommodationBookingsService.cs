using PTimeJobs.Application.Accommodation.Dtos;

namespace PTimeJobs.Application.Accommodation.Interfaces;

public interface IAccommodationBookingsService
{
    Task<AccommodationBookingResponse?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<AccommodationBookingResponse>> GetByWorkerAsync(Guid workerProfileId, CancellationToken cancellationToken = default);

    Task<AccommodationBookingResponse> CreateAsync(CreateAccommodationBookingRequest request, CancellationToken cancellationToken = default);

    Task<AccommodationBookingResponse?> ConfirmAsync(Guid bookingId, CancellationToken cancellationToken = default);

    Task<AccommodationBookingResponse?> CheckInAsync(Guid bookingId, CancellationToken cancellationToken = default);

    Task<AccommodationBookingResponse?> CompleteAsync(Guid bookingId, CompleteBookingRequest request, CancellationToken cancellationToken = default);

    Task<AccommodationBookingResponse?> CancelAsync(Guid bookingId, CancellationToken cancellationToken = default);

    Task<AccommodationBookingResponse?> RejectAsync(Guid bookingId, CancellationToken cancellationToken = default);
}
