using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class JobsController(IJobQueryService jobQueryService, IJobCommandService jobCommandService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<JobSummaryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string? status,
        [FromQuery] Guid? jobCategoryId,
        [FromQuery] string? employmentType,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await jobQueryService.SearchAsync(status, jobCategoryId, employmentType, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<JobSummaryResponse>>.Success(result));
    }

    [HttpGet("{jobId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<JobDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<JobDetailResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid jobId, CancellationToken cancellationToken)
    {
        var job = await jobQueryService.GetByIdAsync(jobId, cancellationToken);

        if (job is null)
        {
            return NotFound(ApiResponse<JobDetailResponse>.Failure("Job not found."));
        }

        return Ok(ApiResponse<JobDetailResponse>.Success(job));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<JobDetailResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateJobRequest request, CancellationToken cancellationToken)
    {
        var job = await jobCommandService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { jobId = job.JobId },
            ApiResponse<JobDetailResponse>.Success(job, "Job created."));
    }

    [HttpPatch("{jobId:guid}/publish")]
    [ProducesResponseType(typeof(ApiResponse<JobDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<JobDetailResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Publish(Guid jobId, CancellationToken cancellationToken)
    {
        var job = await jobCommandService.PublishAsync(jobId, cancellationToken);

        if (job is null)
        {
            return NotFound(ApiResponse<JobDetailResponse>.Failure("Job not found."));
        }

        return Ok(ApiResponse<JobDetailResponse>.Success(job, "Job published."));
    }

    [HttpPatch("{jobId:guid}/close")]
    [ProducesResponseType(typeof(ApiResponse<JobDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<JobDetailResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Close(Guid jobId, CancellationToken cancellationToken)
    {
        var job = await jobCommandService.CloseAsync(jobId, cancellationToken);

        if (job is null)
        {
            return NotFound(ApiResponse<JobDetailResponse>.Failure("Job not found."));
        }

        return Ok(ApiResponse<JobDetailResponse>.Success(job, "Job closed."));
    }
}
