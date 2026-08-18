using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Employers.Dtos;
using PTimeJobs.Application.Employers.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class EmployerProfilesController(
    IEmployerProfileQueryService queryService,
    IEmployerProfileCommandService commandService) : ControllerBase
{
    [HttpGet("{employerProfileId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<EmployerProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<EmployerProfileResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid employerProfileId, CancellationToken cancellationToken)
    {
        var profile = await queryService.GetByIdAsync(employerProfileId, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<EmployerProfileResponse>.Failure("Employer profile not found."));
        }

        return Ok(ApiResponse<EmployerProfileResponse>.Success(profile));
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<EmployerProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<EmployerProfileResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await queryService.GetByUserIdAsync(userId, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<EmployerProfileResponse>.Failure("Employer profile not found."));
        }

        return Ok(ApiResponse<EmployerProfileResponse>.Success(profile));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EmployerProfileResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEmployerProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = await commandService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { employerProfileId = profile.EmployerProfileId },
            ApiResponse<EmployerProfileResponse>.Success(profile, "Employer profile created."));
    }
}
