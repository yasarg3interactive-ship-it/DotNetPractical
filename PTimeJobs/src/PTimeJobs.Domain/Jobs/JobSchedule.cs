namespace PTimeJobs.Domain.Jobs;

public sealed class JobSchedule
{
    private JobSchedule()
    {
    }

    public Guid JobScheduleId { get; private set; }
    public Guid JobId { get; private set; }
    public short? DayOfWeek { get; private set; }
    public TimeOnly? StartTime { get; private set; }
    public TimeOnly? EndTime { get; private set; }
    public DateOnly? StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public string? ShiftLabel { get; private set; }
    public int RequiredWorkers { get; private set; }

    public static JobSchedule Create(
        Guid jobId,
        short? dayOfWeek = null,
        TimeOnly? startTime = null,
        TimeOnly? endTime = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        string? shiftLabel = null,
        int requiredWorkers = 1)
    {
        if (dayOfWeek is < 0 or > 6)
        {
            throw new InvalidOperationException("Day of week must be between 0 (Sunday) and 6 (Saturday).");
        }

        if (startTime.HasValue && endTime.HasValue && startTime >= endTime)
        {
            throw new InvalidOperationException("Start time must be before end time.");
        }

        return new JobSchedule
        {
            JobScheduleId = Guid.NewGuid(),
            JobId = jobId,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            StartDate = startDate,
            EndDate = endDate,
            ShiftLabel = shiftLabel,
            RequiredWorkers = requiredWorkers
        };
    }
}
