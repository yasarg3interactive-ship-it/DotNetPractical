using System.Net;

namespace PTimeJobs.Domain.Analytics;

public sealed class AuditLog
{
    private AuditLog()
    {
    }

    public Guid AuditLogId { get; private set; }
    public Guid? ActorUserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string EntityType { get; private set; } = string.Empty;
    public Guid? EntityId { get; private set; }
    public string? BeforeData { get; private set; }
    public string? AfterData { get; private set; }
    public IPAddress? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static AuditLog Create(
        string action,
        string entityType,
        Guid? actorUserId = null,
        Guid? entityId = null,
        string? beforeData = null,
        string? afterData = null,
        IPAddress? ipAddress = null,
        string? userAgent = null)
    {
        if (string.IsNullOrWhiteSpace(action))
        {
            throw new InvalidOperationException("Action is required.");
        }

        if (string.IsNullOrWhiteSpace(entityType))
        {
            throw new InvalidOperationException("Entity type is required.");
        }

        return new AuditLog
        {
            AuditLogId = Guid.NewGuid(),
            ActorUserId = actorUserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            BeforeData = beforeData,
            AfterData = afterData,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
