using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Analytics.Dtos;
using PTimeJobs.Application.Analytics.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class UserBehaviorEventsController(IAnalyticsEventsService analyticsEventsService) : ControllerBase
{
    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<UserBehaviorEventResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var events = await analyticsEventsService.GetBehaviorEventsByUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<UserBehaviorEventResponse>>.Success(events));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UserBehaviorEventResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserBehaviorEventRequest request, CancellationToken cancellationToken)
    {
        var behaviorEvent = await analyticsEventsService.CreateBehaviorEventAsync(request, cancellationToken);
        return Created(string.Empty, ApiResponse<UserBehaviorEventResponse>.Success(behaviorEvent, "Event recorded."));
    }
}
