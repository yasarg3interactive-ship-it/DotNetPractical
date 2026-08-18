namespace PTimeJobs.Application.Notifications.Dtos;

public sealed record NotificationResponse(
    Guid NotificationId,
    Guid UserId,
    string NotificationType,
    string Title,
    string? Body,
    string Status,
    string? EntityType,
    Guid? EntityId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? SentAt,
    DateTimeOffset? ReadAt);

public sealed record CreateNotificationRequest(
    Guid UserId,
    string NotificationType,
    string Title,
    string? Body,
    string? EntityType,
    Guid? EntityId);
