namespace PTimeJobs.Domain.Jobs;

public sealed class HiringStatusHistory
{
    private HiringStatusHistory()
    {
    }

    public Guid HiringStatusHistoryId { get; private set; }
    public Guid ApplicationId { get; private set; }
    public ApplicationStatus? OldStatus { get; private set; }
    public ApplicationStatus NewStatus { get; private set; }
    public Guid? ChangedBy { get; private set; }
    public string? Reason { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static HiringStatusHistory Create(
        Guid applicationId,
        ApplicationStatus? oldStatus,
        ApplicationStatus newStatus,
        Guid? changedBy = null,
        string? reason = null)
    {
        return new HiringStatusHistory
        {
            HiringStatusHistoryId = Guid.NewGuid(),
            ApplicationId = applicationId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            ChangedBy = changedBy,
            Reason = reason,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
