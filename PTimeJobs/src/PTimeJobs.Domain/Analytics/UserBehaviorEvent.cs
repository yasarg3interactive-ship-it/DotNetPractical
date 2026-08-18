namespace PTimeJobs.Domain.Analytics;

public sealed class UserBehaviorEvent
{
    private UserBehaviorEvent()
    {
    }

    public Guid BehaviorEventId { get; private set; }
    public Guid? UserId { get; private set; }
    public string EventName { get; private set; } = string.Empty;
    public string? EntityType { get; private set; }
    public Guid? EntityId { get; private set; }
    public string EventProperties { get; private set; } = "{}";
    public DateTimeOffset OccurredAt { get; private set; }

    public static UserBehaviorEvent Create(
        string eventName,
        Guid? userId = null,
        string? entityType = null,
        Guid? entityId = null)
    {
        if (string.IsNullOrWhiteSpace(eventName))
        {
            throw new InvalidOperationException("Event name is required.");
        }

        return new UserBehaviorEvent
        {
            BehaviorEventId = Guid.NewGuid(),
            UserId = userId,
            EventName = eventName,
            EntityType = entityType,
            EntityId = entityId,
            OccurredAt = DateTimeOffset.UtcNow
        };
    }
}
