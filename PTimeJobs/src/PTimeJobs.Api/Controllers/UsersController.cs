using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class UsersController(IUserQueryService userQueryService, IRbacService rbacService) : ControllerBase
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

    [HttpGet("{userId:guid}/roles")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<UserRoleResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoles(Guid userId, CancellationToken cancellationToken)
    {
        var roles = await rbacService.GetUserRolesAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<UserRoleResponse>>.Success(roles));
    }

    [HttpPost("{userId:guid}/roles/{roleId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserRoleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<UserRoleResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignRole(Guid userId, Guid roleId, [FromBody] AssignRoleRequest request, CancellationToken cancellationToken)
    {
        var userRole = await rbacService.AssignRoleToUserAsync(userId, roleId, request, cancellationToken);

        if (userRole is null)
        {
            return NotFound(ApiResponse<UserRoleResponse>.Failure("User not found."));
        }

        return CreatedAtAction(nameof(GetRoles), new { userId }, ApiResponse<UserRoleResponse>.Success(userRole, "Role assigned."));
    }

    [HttpDelete("{userId:guid}/roles/{roleId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveRole(Guid userId, Guid roleId, CancellationToken cancellationToken)
    {
        var removed = await rbacService.RemoveRoleFromUserAsync(userId, roleId, cancellationToken);

        if (!removed)
        {
            return NotFound(ApiResponse<object>.Failure("Role assignment not found."));
        }

        return Ok(ApiResponse<object>.Success(null, "Role removed."));
    }
}
