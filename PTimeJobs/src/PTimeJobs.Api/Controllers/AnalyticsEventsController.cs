using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Analytics.Dtos;
using PTimeJobs.Application.Analytics.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class AnalyticsEventsController(IAnalyticsEventsService analyticsEventsService) : ControllerBase
{
    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<AnalyticsEventResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var events = await analyticsEventsService.GetAnalyticsEventsByUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<AnalyticsEventResponse>>.Success(events));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AnalyticsEventResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAnalyticsEventRequest request, CancellationToken cancellationToken)
    {
        var analyticsEvent = await analyticsEventsService.CreateAnalyticsEventAsync(request, cancellationToken);
        return Created(string.Empty, ApiResponse<AnalyticsEventResponse>.Success(analyticsEvent, "Event recorded."));
    }
}
