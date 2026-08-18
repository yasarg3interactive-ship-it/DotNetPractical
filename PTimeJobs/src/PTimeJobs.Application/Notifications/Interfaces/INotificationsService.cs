using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Notifications.Dtos;

namespace PTimeJobs.Application.Notifications.Interfaces;

public interface INotificationsService
{
    Task<NotificationResponse?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default);

    Task<PagedResult<NotificationResponse>> GetByUserAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<NotificationResponse> CreateAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default);

    Task<NotificationResponse?> MarkSentAsync(Guid notificationId, CancellationToken cancellationToken = default);

    Task<NotificationResponse?> MarkReadAsync(Guid notificationId, CancellationToken cancellationToken = default);
}
