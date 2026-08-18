namespace PTimeJobs.Domain.Workers;

public sealed class WorkerAvailability
{
    private WorkerAvailability()
    {
    }

    public Guid AvailabilityId { get; private set; }
    public Guid WorkerProfileId { get; private set; }
    public short DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public DateOnly? EffectiveFrom { get; private set; }
    public DateOnly? EffectiveTo { get; private set; }
    public bool IsAvailable { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static WorkerAvailability Create(
        Guid workerProfileId,
        short dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        DateOnly? effectiveFrom = null,
        DateOnly? effectiveTo = null,
        bool isAvailable = true)
    {
        if (dayOfWeek is < 0 or > 6)
        {
            throw new InvalidOperationException("Day of week must be between 0 (Sunday) and 6 (Saturday).");
        }

        if (startTime >= endTime)
        {
            throw new InvalidOperationException("Start time must be before end time.");
        }

        return new WorkerAvailability
        {
            AvailabilityId = Guid.NewGuid(),
            WorkerProfileId = workerProfileId,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            IsAvailable = isAvailable,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
