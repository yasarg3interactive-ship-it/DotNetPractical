using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Analytics.Dtos;
using PTimeJobs.Application.Analytics.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class UserPreferencesController(IPersonalizationService personalizationService) : ControllerBase
{
    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<UserPreferenceResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var preferences = await personalizationService.GetPreferencesAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<UserPreferenceResponse>>.Success(preferences));
    }

    [HttpPut("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserPreferenceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upsert(Guid userId, [FromBody] UpsertUserPreferenceRequest request, CancellationToken cancellationToken)
    {
        var preference = await personalizationService.UpsertPreferenceAsync(userId, request, cancellationToken);
        return Ok(ApiResponse<UserPreferenceResponse>.Success(preference, "Preferences saved."));
    }
}
