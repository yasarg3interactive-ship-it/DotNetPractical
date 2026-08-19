using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class UserProfilesController(IUserProfilesService userProfilesService) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserProfileResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await userProfilesService.GetByUserIdAsync(userId, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<UserProfileResponse>.Failure("Profile not found."));
        }

        return Ok(ApiResponse<UserProfileResponse>.Success(profile));
    }

    [HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upsert(Guid userId, [FromBody] UpdateUserProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = await userProfilesService.UpsertAsync(userId, request, cancellationToken);
        return Ok(ApiResponse<UserProfileResponse>.Success(profile, "Profile saved."));
    }
}
