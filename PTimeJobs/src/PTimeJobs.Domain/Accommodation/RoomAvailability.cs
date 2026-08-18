namespace PTimeJobs.Domain.Accommodation;

public sealed class RoomAvailability
{
    private RoomAvailability()
    {
    }

    public Guid RoomAvailabilityId { get; private set; }
    public Guid RoomId { get; private set; }
    public DateOnly AvailableFrom { get; private set; }
    public DateOnly? AvailableTo { get; private set; }
    public int AvailableBeds { get; private set; }
    public decimal? PriceOverride { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static RoomAvailability Create(
        Guid roomId,
        DateOnly availableFrom,
        int availableBeds,
        DateOnly? availableTo = null,
        decimal? priceOverride = null)
    {
        if (availableBeds < 0)
        {
            throw new InvalidOperationException("Available beds cannot be negative.");
        }

        return new RoomAvailability
        {
            RoomAvailabilityId = Guid.NewGuid(),
            RoomId = roomId,
            AvailableFrom = availableFrom,
            AvailableTo = availableTo,
            AvailableBeds = availableBeds,
            PriceOverride = priceOverride,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
