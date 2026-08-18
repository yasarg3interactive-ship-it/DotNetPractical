namespace PTimeJobs.Domain.Accommodation;

public sealed class AccommodationBooking
{
    private AccommodationBooking()
    {
    }

    public Guid BookingId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid WorkerProfileId { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateOnly CheckInDate { get; private set; }
    public DateOnly? CheckOutDate { get; private set; }
    public decimal? TotalAmount { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static AccommodationBooking Create(
        Guid roomId,
        Guid workerProfileId,
        DateOnly checkInDate,
        decimal? totalAmount = null)
    {
        return new AccommodationBooking
        {
            BookingId = Guid.NewGuid(),
            RoomId = roomId,
            WorkerProfileId = workerProfileId,
            Status = BookingStatus.Requested,
            CheckInDate = checkInDate,
            TotalAmount = totalAmount,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Confirm()
    {
        Status = BookingStatus.Confirmed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void CheckIn()
    {
        Status = BookingStatus.CheckedIn;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Complete(DateOnly checkOutDate)
    {
        Status = BookingStatus.Completed;
        CheckOutDate = checkOutDate;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        Status = BookingStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Reject()
    {
        Status = BookingStatus.Rejected;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
