using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Notifications.Dtos;
using PTimeJobs.Application.Notifications.Interfaces;
using PTimeJobs.Domain.Notifications;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Notifications;

public sealed class NotificationsService(ApplicationDbContext dbContext) : INotificationsService
{
    public async Task<NotificationResponse?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await dbContext.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.NotificationId == notificationId, cancellationToken);
        return notification is null ? null : ToResponse(notification);
    }

    public async Task<PagedResult<NotificationResponse>> GetByUserAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var query = dbContext.Notifications.AsNoTracking().Where(n => n.UserId == userId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<NotificationResponse>(items.Select(ToResponse).ToList(), page, pageSize, totalCount);
    }

    public async Task<NotificationResponse> CreateAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var notification = Notification.Create(
            request.UserId,
            request.NotificationType,
            request.Title,
            request.Body,
            request.EntityType,
            request.EntityId);

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(notification);
    }

    public async Task<NotificationResponse?> MarkSentAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await dbContext.Notifications.FirstOrDefaultAsync(n => n.NotificationId == notificationId, cancellationToken);
        if (notification is null)
        {
            return null;
        }

        notification.MarkSent();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(notification);
    }

    public async Task<NotificationResponse?> MarkReadAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await dbContext.Notifications.FirstOrDefaultAsync(n => n.NotificationId == notificationId, cancellationToken);
        if (notification is null)
        {
            return null;
        }

        notification.MarkRead();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(notification);
    }

    private static NotificationResponse ToResponse(Notification notification) => new(
        notification.NotificationId,
        notification.UserId,
        notification.NotificationType,
        notification.Title,
        notification.Body,
        notification.Status.ToString(),
        notification.EntityType,
        notification.EntityId,
        notification.CreatedAt,
        notification.SentAt,
        notification.ReadAt);
}
