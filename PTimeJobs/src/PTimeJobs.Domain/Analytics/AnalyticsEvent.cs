namespace PTimeJobs.Domain.Analytics;

public sealed class AnalyticsEvent
{
    private AnalyticsEvent()
    {
    }

    public Guid AnalyticsEventId { get; private set; }
    public Guid? UserId { get; private set; }
    public string? AnonymousId { get; private set; }
    public string EventName { get; private set; } = string.Empty;
    public string? Source { get; private set; }
    public Guid? SessionId { get; private set; }
    public string? EntityType { get; private set; }
    public Guid? EntityId { get; private set; }
    public string Properties { get; private set; } = "{}";
    public DateTimeOffset OccurredAt { get; private set; }

    public static AnalyticsEvent Create(
        string eventName,
        Guid? userId = null,
        string? anonymousId = null,
        string? source = null,
        Guid? sessionId = null,
        string? entityType = null,
        Guid? entityId = null)
    {
        if (string.IsNullOrWhiteSpace(eventName))
        {
            throw new InvalidOperationException("Event name is required.");
        }

        return new AnalyticsEvent
        {
            AnalyticsEventId = Guid.NewGuid(),
            UserId = userId,
            AnonymousId = anonymousId,
            EventName = eventName,
            Source = source,
            SessionId = sessionId,
            EntityType = entityType,
            EntityId = entityId,
            OccurredAt = DateTimeOffset.UtcNow
        };
    }
}
