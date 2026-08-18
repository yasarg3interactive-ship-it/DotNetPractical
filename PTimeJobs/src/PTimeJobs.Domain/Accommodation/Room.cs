namespace PTimeJobs.Domain.Accommodation;

public sealed class Room
{
    private Room()
    {
    }

    public Guid RoomId { get; private set; }
    public Guid PropertyId { get; private set; }
    public Guid? RoomTypeId { get; private set; }
    public string? RoomNumber { get; private set; }
    public string? FloorNumber { get; private set; }
    public int Capacity { get; private set; }
    public int OccupiedCount { get; private set; }
    public decimal MonthlyPrice { get; private set; }
    public decimal? SecurityDeposit { get; private set; }
    public bool IsAvailable { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static Room Create(
        Guid propertyId,
        int capacity,
        decimal monthlyPrice,
        Guid? roomTypeId = null,
        string? roomNumber = null,
        string? floorNumber = null,
        decimal? securityDeposit = null)
    {
        if (capacity <= 0)
        {
            throw new InvalidOperationException("Capacity must be greater than zero.");
        }

        return new Room
        {
            RoomId = Guid.NewGuid(),
            PropertyId = propertyId,
            RoomTypeId = roomTypeId,
            RoomNumber = roomNumber,
            FloorNumber = floorNumber,
            Capacity = capacity,
            OccupiedCount = 0,
            MonthlyPrice = monthlyPrice,
            SecurityDeposit = securityDeposit,
            IsAvailable = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Occupy()
    {
        if (OccupiedCount >= Capacity)
        {
            throw new InvalidOperationException("Room is already at full capacity.");
        }

        OccupiedCount++;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Vacate()
    {
        if (OccupiedCount <= 0)
        {
            throw new InvalidOperationException("Room has no occupants to vacate.");
        }

        OccupiedCount--;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
