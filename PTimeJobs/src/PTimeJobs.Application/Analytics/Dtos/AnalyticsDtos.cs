namespace PTimeJobs.Application.Analytics.Dtos;

public sealed record AuditLogResponse(
    Guid AuditLogId,
    Guid? ActorUserId,
    string Action,
    string EntityType,
    Guid? EntityId,
    DateTimeOffset CreatedAt);

public sealed record CreateAuditLogRequest(string Action, string EntityType, Guid? ActorUserId, Guid? EntityId, string? BeforeData, string? AfterData);

public sealed record AnalyticsEventResponse(
    Guid AnalyticsEventId,
    Guid? UserId,
    string? AnonymousId,
    string EventName,
    string? Source,
    DateTimeOffset OccurredAt);

public sealed record CreateAnalyticsEventRequest(string EventName, Guid? UserId, string? AnonymousId, string? Source, Guid? SessionId, string? EntityType, Guid? EntityId);

public sealed record UserBehaviorEventResponse(Guid BehaviorEventId, Guid? UserId, string EventName, string? EntityType, Guid? EntityId, DateTimeOffset OccurredAt);

public sealed record CreateUserBehaviorEventRequest(string EventName, Guid? UserId, string? EntityType, Guid? EntityId);

public sealed record SearchHistoryResponse(
    Guid SearchId,
    Guid? UserId,
    string SearchScope,
    string? QueryText,
    int? ResultCount,
    DateTimeOffset CreatedAt);

public sealed record CreateSearchHistoryRequest(string SearchScope, Guid? UserId, string? QueryText, int? ResultCount, Guid? LocationId);

public sealed record RecommendationHistoryResponse(
    Guid RecommendationId,
    Guid UserId,
    string RecommendationType,
    string TargetEntityType,
    Guid TargetEntityId,
    decimal? Score,
    DateTimeOffset ShownAt,
    DateTimeOffset? ClickedAt,
    DateTimeOffset? DismissedAt,
    DateTimeOffset? ConvertedAt);

public sealed record CreateRecommendationRequest(
    Guid UserId,
    string RecommendationType,
    string TargetEntityType,
    Guid TargetEntityId,
    decimal? Score,
    string? ModelVersion);

public sealed record UserPreferenceResponse(Guid PreferenceId, Guid UserId, string PreferenceScope, string Preferences, DateTimeOffset UpdatedAt);

public sealed record UpsertUserPreferenceRequest(string PreferenceScope, string Preferences);
