using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Analytics.Dtos;
using PTimeJobs.Application.Analytics.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class AuditLogsController(IAnalyticsEventsService analyticsEventsService) : ControllerBase
{
    [HttpGet("by-user/{actorUserId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<AuditLogResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid actorUserId, CancellationToken cancellationToken)
    {
        var logs = await analyticsEventsService.GetAuditLogsByUserAsync(actorUserId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<AuditLogResponse>>.Success(logs));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AuditLogResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAuditLogRequest request, CancellationToken cancellationToken)
    {
        var log = await analyticsEventsService.CreateAuditLogAsync(request, cancellationToken);
        return Created(string.Empty, ApiResponse<AuditLogResponse>.Success(log, "Audit log recorded."));
    }
}
