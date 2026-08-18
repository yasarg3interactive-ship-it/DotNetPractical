using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class JobApplicationsController(
    IJobApplicationQueryService jobApplicationQueryService,
    IJobApplicationCommandService jobApplicationCommandService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<JobApplicationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] Guid? jobId,
        [FromQuery] Guid? workerProfileId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await jobApplicationQueryService.SearchAsync(jobId, workerProfileId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<JobApplicationResponse>>.Success(result));
    }

    [HttpGet("{applicationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<JobApplicationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<JobApplicationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await jobApplicationQueryService.GetByIdAsync(applicationId, cancellationToken);

        if (application is null)
        {
            return NotFound(ApiResponse<JobApplicationResponse>.Failure("Application not found."));
        }

        return Ok(ApiResponse<JobApplicationResponse>.Success(application));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<JobApplicationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateJobApplicationRequest request, CancellationToken cancellationToken)
    {
        var application = await jobApplicationCommandService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { applicationId = application.ApplicationId },
            ApiResponse<JobApplicationResponse>.Success(application, "Application submitted."));
    }

    [HttpPatch("{applicationId:guid}/status")]
    [ProducesResponseType(typeof(ApiResponse<JobApplicationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<JobApplicationResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus(
        Guid applicationId,
        [FromBody] UpdateJobApplicationStatusRequest request,
        CancellationToken cancellationToken)
    {
        var application = await jobApplicationCommandService.UpdateStatusAsync(applicationId, request.Status, cancellationToken);

        if (application is null)
        {
            return NotFound(ApiResponse<JobApplicationResponse>.Failure("Application not found."));
        }

        return Ok(ApiResponse<JobApplicationResponse>.Success(application, "Application status updated."));
    }
}
