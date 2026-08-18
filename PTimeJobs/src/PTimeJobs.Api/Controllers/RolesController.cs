using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class RolesController(IRbacService rbacService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<RoleResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var roles = await rbacService.GetRolesAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<RoleResponse>>.Success(roles));
    }

    [HttpGet("{roleId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<RoleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<RoleResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await rbacService.GetRoleByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return NotFound(ApiResponse<RoleResponse>.Failure("Role not found."));
        }

        return Ok(ApiResponse<RoleResponse>.Success(role));
    }

    [HttpGet("{roleId:guid}/permissions")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<PermissionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPermissions(Guid roleId, CancellationToken cancellationToken)
    {
        var permissions = await rbacService.GetRolePermissionsAsync(roleId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<PermissionResponse>>.Success(permissions));
    }

    [HttpPost("{roleId:guid}/permissions/{permissionId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignPermission(Guid roleId, Guid permissionId, CancellationToken cancellationToken)
    {
        var assigned = await rbacService.AssignPermissionToRoleAsync(roleId, permissionId, cancellationToken);

        if (!assigned)
        {
            return NotFound(ApiResponse<object>.Failure("Role not found."));
        }

        return Ok(ApiResponse<object>.Success(null, "Permission assigned to role."));
    }
}
