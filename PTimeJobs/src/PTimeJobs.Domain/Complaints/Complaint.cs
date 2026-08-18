namespace PTimeJobs.Domain.Complaints;

public sealed class Complaint
{
    private Complaint()
    {
    }

    public Guid ComplaintId { get; private set; }
    public Guid ComplainantUserId { get; private set; }
    public string TargetEntityType { get; private set; } = string.Empty;
    public Guid TargetEntityId { get; private set; }
    public string ComplaintCategory { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ComplaintStatus Status { get; private set; }
    public Guid? AssignedTo { get; private set; }
    public string? ResolutionNotes { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }

    public static Complaint Create(
        Guid complainantUserId,
        string targetEntityType,
        Guid targetEntityId,
        string complaintCategory,
        string description)
    {
        if (string.IsNullOrWhiteSpace(targetEntityType))
        {
            throw new InvalidOperationException("Target entity type is required.");
        }

        if (string.IsNullOrWhiteSpace(complaintCategory))
        {
            throw new InvalidOperationException("Complaint category is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidOperationException("Description is required.");
        }

        return new Complaint
        {
            ComplaintId = Guid.NewGuid(),
            ComplainantUserId = complainantUserId,
            TargetEntityType = targetEntityType,
            TargetEntityId = targetEntityId,
            ComplaintCategory = complaintCategory,
            Description = description,
            Status = ComplaintStatus.Open,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Assign(Guid assignedTo)
    {
        AssignedTo = assignedTo;
        Status = ComplaintStatus.InReview;
    }

    public void Resolve(string resolutionNotes)
    {
        Status = ComplaintStatus.Resolved;
        ResolutionNotes = resolutionNotes;
        ResolvedAt = DateTimeOffset.UtcNow;
    }

    public void Reject(string resolutionNotes)
    {
        Status = ComplaintStatus.Rejected;
        ResolutionNotes = resolutionNotes;
        ResolvedAt = DateTimeOffset.UtcNow;
    }

    public void Escalate()
    {
        Status = ComplaintStatus.Escalated;
    }
}
