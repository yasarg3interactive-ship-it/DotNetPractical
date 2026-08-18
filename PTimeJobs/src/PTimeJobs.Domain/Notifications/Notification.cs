namespace PTimeJobs.Domain.Notifications;

public sealed class Notification
{
    private Notification()
    {
    }

    public Guid NotificationId { get; private set; }
    public Guid UserId { get; private set; }
    public string NotificationType { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string? Body { get; private set; }
    public NotificationStatus Status { get; private set; }
    public string? EntityType { get; private set; }
    public Guid? EntityId { get; private set; }
    public string Metadata { get; private set; } = "{}";
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public DateTimeOffset? ReadAt { get; private set; }

    public static Notification Create(
        Guid userId,
        string notificationType,
        string title,
        string? body = null,
        string? entityType = null,
        Guid? entityId = null)
    {
        if (string.IsNullOrWhiteSpace(notificationType))
        {
            throw new InvalidOperationException("Notification type is required.");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidOperationException("Title is required.");
        }

        return new Notification
        {
            NotificationId = Guid.NewGuid(),
            UserId = userId,
            NotificationType = notificationType,
            Title = title,
            Body = body,
            Status = NotificationStatus.Queued,
            EntityType = entityType,
            EntityId = entityId,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void MarkSent()
    {
        Status = NotificationStatus.Sent;
        SentAt = DateTimeOffset.UtcNow;
    }

    public void MarkRead()
    {
        Status = NotificationStatus.Read;
        ReadAt = DateTimeOffset.UtcNow;
    }

    public void MarkFailed()
    {
        Status = NotificationStatus.Failed;
    }
}
