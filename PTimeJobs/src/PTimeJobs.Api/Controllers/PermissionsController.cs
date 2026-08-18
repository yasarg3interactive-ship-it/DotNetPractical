using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class PermissionsController(IRbacService rbacService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<PermissionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var permissions = await rbacService.GetPermissionsAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<PermissionResponse>>.Success(permissions));
    }

    [HttpGet("{permissionId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PermissionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PermissionResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid permissionId, CancellationToken cancellationToken)
    {
        var permission = await rbacService.GetPermissionByIdAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            return NotFound(ApiResponse<PermissionResponse>.Failure("Permission not found."));
        }

        return Ok(ApiResponse<PermissionResponse>.Success(permission));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PermissionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var permission = await rbacService.CreatePermissionAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { permissionId = permission.PermissionId },
            ApiResponse<PermissionResponse>.Success(permission, "Permission created."));
    }
}
