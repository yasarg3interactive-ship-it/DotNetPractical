using PTimeJobs.Application.Analytics.Dtos;

namespace PTimeJobs.Application.Analytics.Interfaces;

public interface IAnalyticsEventsService
{
    Task<AuditLogResponse> CreateAuditLogAsync(CreateAuditLogRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<AuditLogResponse>> GetAuditLogsByUserAsync(Guid actorUserId, CancellationToken cancellationToken = default);

    Task<AnalyticsEventResponse> CreateAnalyticsEventAsync(CreateAnalyticsEventRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<AnalyticsEventResponse>> GetAnalyticsEventsByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<UserBehaviorEventResponse> CreateBehaviorEventAsync(CreateUserBehaviorEventRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<UserBehaviorEventResponse>> GetBehaviorEventsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
