using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Notifications.Dtos;
using PTimeJobs.Application.Notifications.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class NotificationsController(INotificationsService notificationsService) : ControllerBase
{
    [HttpGet("{notificationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<NotificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NotificationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await notificationsService.GetByIdAsync(notificationId, cancellationToken);

        if (notification is null)
        {
            return NotFound(ApiResponse<NotificationResponse>.Failure("Notification not found."));
        }

        return Ok(ApiResponse<NotificationResponse>.Success(notification));
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<NotificationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(
        Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var notifications = await notificationsService.GetByUserAsync(userId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<NotificationResponse>>.Success(notifications));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NotificationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request, CancellationToken cancellationToken)
    {
        var notification = await notificationsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { notificationId = notification.NotificationId },
            ApiResponse<NotificationResponse>.Success(notification, "Notification created."));
    }

    [HttpPatch("{notificationId:guid}/mark-sent")]
    [ProducesResponseType(typeof(ApiResponse<NotificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NotificationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkSent(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await notificationsService.MarkSentAsync(notificationId, cancellationToken);

        if (notification is null)
        {
            return NotFound(ApiResponse<NotificationResponse>.Failure("Notification not found."));
        }

        return Ok(ApiResponse<NotificationResponse>.Success(notification, "Marked as sent."));
    }

    [HttpPatch("{notificationId:guid}/mark-read")]
    [ProducesResponseType(typeof(ApiResponse<NotificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NotificationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkRead(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await notificationsService.MarkReadAsync(notificationId, cancellationToken);

        if (notification is null)
        {
            return NotFound(ApiResponse<NotificationResponse>.Failure("Notification not found."));
        }

        return Ok(ApiResponse<NotificationResponse>.Success(notification, "Marked as read."));
    }
}
