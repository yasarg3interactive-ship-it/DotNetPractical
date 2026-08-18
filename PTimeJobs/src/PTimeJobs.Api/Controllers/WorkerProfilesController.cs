using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Workers.Dtos;
using PTimeJobs.Application.Workers.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class WorkerProfilesController(
    IWorkerProfileQueryService queryService,
    IWorkerProfileCommandService commandService) : ControllerBase
{
    [HttpGet("{workerProfileId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid workerProfileId, CancellationToken cancellationToken)
    {
        var profile = await queryService.GetByIdAsync(workerProfileId, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<WorkerProfileResponse>.Failure("Worker profile not found."));
        }

        return Ok(ApiResponse<WorkerProfileResponse>.Success(profile));
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await queryService.GetByUserIdAsync(userId, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<WorkerProfileResponse>.Failure("Worker profile not found."));
        }

        return Ok(ApiResponse<WorkerProfileResponse>.Success(profile));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateWorkerProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = await commandService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { workerProfileId = profile.WorkerProfileId },
            ApiResponse<WorkerProfileResponse>.Success(profile, "Worker profile created."));
    }

    [HttpPatch("{workerProfileId:guid}/headline")]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHeadline(
        Guid workerProfileId,
        [FromBody] UpdateWorkerHeadlineRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await commandService.UpdateHeadlineAsync(workerProfileId, request, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<WorkerProfileResponse>.Failure("Worker profile not found."));
        }

        return Ok(ApiResponse<WorkerProfileResponse>.Success(profile, "Headline updated."));
    }

    [HttpPatch("{workerProfileId:guid}/expected-salary")]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateExpectedSalary(
        Guid workerProfileId,
        [FromBody] UpdateWorkerExpectedSalaryRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await commandService.UpdateExpectedSalaryAsync(workerProfileId, request, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<WorkerProfileResponse>.Failure("Worker profile not found."));
        }

        return Ok(ApiResponse<WorkerProfileResponse>.Success(profile, "Expected salary updated."));
    }

    [HttpPost("{workerProfileId:guid}/skills")]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSkill(
        Guid workerProfileId,
        [FromBody] AddWorkerSkillRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await commandService.AddSkillAsync(workerProfileId, request, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<WorkerProfileResponse>.Failure("Worker profile not found."));
        }

        return Ok(ApiResponse<WorkerProfileResponse>.Success(profile, "Skill added."));
    }

    [HttpPost("{workerProfileId:guid}/experience")]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddExperience(
        Guid workerProfileId,
        [FromBody] AddWorkerExperienceRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await commandService.AddExperienceAsync(workerProfileId, request, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<WorkerProfileResponse>.Failure("Worker profile not found."));
        }

        return Ok(ApiResponse<WorkerProfileResponse>.Success(profile, "Experience added."));
    }

    [HttpPost("{workerProfileId:guid}/education")]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<WorkerProfileResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddEducation(
        Guid workerProfileId,
        [FromBody] AddWorkerEducationRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await commandService.AddEducationAsync(workerProfileId, request, cancellationToken);

        if (profile is null)
        {
            return NotFound(ApiResponse<WorkerProfileResponse>.Failure("Worker profile not found."));
        }

        return Ok(ApiResponse<WorkerProfileResponse>.Success(profile, "Education added."));
    }
}
