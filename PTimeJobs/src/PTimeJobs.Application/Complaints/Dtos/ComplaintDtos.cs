namespace PTimeJobs.Application.Complaints.Dtos;

public sealed record ComplaintResponse(
    Guid ComplaintId,
    Guid ComplainantUserId,
    string TargetEntityType,
    Guid TargetEntityId,
    string ComplaintCategory,
    string Description,
    string Status,
    Guid? AssignedTo,
    string? ResolutionNotes,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ResolvedAt);

public sealed record CreateComplaintRequest(
    Guid ComplainantUserId,
    string TargetEntityType,
    Guid TargetEntityId,
    string ComplaintCategory,
    string Description);

public sealed record ResolveComplaintRequest(string ResolutionNotes);
