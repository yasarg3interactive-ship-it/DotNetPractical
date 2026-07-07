using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class UsersController(IUserQueryService userQueryService) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserSummaryResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userQueryService.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return NotFound(ApiResponse<UserSummaryResponse>.Failure("User not found."));
        }

        return Ok(ApiResponse<UserSummaryResponse>.Success(user));
    }
}
