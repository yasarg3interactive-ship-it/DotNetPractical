using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Analytics.Dtos;
using PTimeJobs.Application.Analytics.Interfaces;
using PTimeJobs.Domain.Analytics;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Analytics;

public sealed class AnalyticsEventsService(ApplicationDbContext dbContext) : IAnalyticsEventsService
{
    public async Task<AuditLogResponse> CreateAuditLogAsync(CreateAuditLogRequest request, CancellationToken cancellationToken = default)
    {
        var log = AuditLog.Create(request.Action, request.EntityType, request.ActorUserId, request.EntityId, request.BeforeData, request.AfterData);
        dbContext.AuditLogs.Add(log);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(log);
    }

    public async Task<IReadOnlyCollection<AuditLogResponse>> GetAuditLogsByUserAsync(Guid actorUserId, CancellationToken cancellationToken = default)
    {
        var logs = await dbContext.AuditLogs
            .AsNoTracking()
            .Where(log => log.ActorUserId == actorUserId)
            .OrderByDescending(log => log.CreatedAt)
            .ToListAsync(cancellationToken);

        return logs.Select(ToResponse).ToList();
    }

    public async Task<AnalyticsEventResponse> CreateAnalyticsEventAsync(CreateAnalyticsEventRequest request, CancellationToken cancellationToken = default)
    {
        var analyticsEvent = AnalyticsEvent.Create(
            request.EventName,
            request.UserId,
            request.AnonymousId,
            request.Source,
            request.SessionId,
            request.EntityType,
            request.EntityId);

        dbContext.AnalyticsEvents.Add(analyticsEvent);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(analyticsEvent);
    }

    public async Task<IReadOnlyCollection<AnalyticsEventResponse>> GetAnalyticsEventsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var events = await dbContext.AnalyticsEvents
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.OccurredAt)
            .ToListAsync(cancellationToken);

        return events.Select(ToResponse).ToList();
    }

    public async Task<UserBehaviorEventResponse> CreateBehaviorEventAsync(CreateUserBehaviorEventRequest request, CancellationToken cancellationToken = default)
    {
        var behaviorEvent = UserBehaviorEvent.Create(request.EventName, request.UserId, request.EntityType, request.EntityId);
        dbContext.UserBehaviorEvents.Add(behaviorEvent);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(behaviorEvent);
    }

    public async Task<IReadOnlyCollection<UserBehaviorEventResponse>> GetBehaviorEventsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var events = await dbContext.UserBehaviorEvents
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.OccurredAt)
            .ToListAsync(cancellationToken);

        return events.Select(ToResponse).ToList();
    }

    private static AuditLogResponse ToResponse(AuditLog log) => new(
        log.AuditLogId,
        log.ActorUserId,
        log.Action,
        log.EntityType,
        log.EntityId,
        log.CreatedAt);

    private static AnalyticsEventResponse ToResponse(AnalyticsEvent analyticsEvent) => new(
        analyticsEvent.AnalyticsEventId,
        analyticsEvent.UserId,
        analyticsEvent.AnonymousId,
        analyticsEvent.EventName,
        analyticsEvent.Source,
        analyticsEvent.OccurredAt);

    private static UserBehaviorEventResponse ToResponse(UserBehaviorEvent behaviorEvent) => new(
        behaviorEvent.BehaviorEventId,
        behaviorEvent.UserId,
        behaviorEvent.EventName,
        behaviorEvent.EntityType,
        behaviorEvent.EntityId,
        behaviorEvent.OccurredAt);
}
