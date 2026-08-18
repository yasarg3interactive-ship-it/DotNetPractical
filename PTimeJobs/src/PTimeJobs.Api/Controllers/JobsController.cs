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

    [HttpGet("{jobId:guid}/locations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<JobLocationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations(Guid jobId, CancellationToken cancellationToken)
    {
        var locations = await jobQueryService.GetLocationsAsync(jobId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<JobLocationResponse>>.Success(locations));
    }

    [HttpPost("{jobId:guid}/locations")]
    [ProducesResponseType(typeof(ApiResponse<JobLocationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<JobLocationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddLocation(Guid jobId, [FromBody] AddJobLocationRequest request, CancellationToken cancellationToken)
    {
        var location = await jobCommandService.AddLocationAsync(jobId, request, cancellationToken);

        if (location is null)
        {
            return NotFound(ApiResponse<JobLocationResponse>.Failure("Job not found."));
        }

        return CreatedAtAction(nameof(GetLocations), new { jobId }, ApiResponse<JobLocationResponse>.Success(location, "Location added."));
    }

    [HttpGet("{jobId:guid}/schedules")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<JobScheduleResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchedules(Guid jobId, CancellationToken cancellationToken)
    {
        var schedules = await jobQueryService.GetSchedulesAsync(jobId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<JobScheduleResponse>>.Success(schedules));
    }

    [HttpPost("{jobId:guid}/schedules")]
    [ProducesResponseType(typeof(ApiResponse<JobScheduleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<JobScheduleResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSchedule(Guid jobId, [FromBody] AddJobScheduleRequest request, CancellationToken cancellationToken)
    {
        var schedule = await jobCommandService.AddScheduleAsync(jobId, request, cancellationToken);

        if (schedule is null)
        {
            return NotFound(ApiResponse<JobScheduleResponse>.Failure("Job not found."));
        }

        return CreatedAtAction(nameof(GetSchedules), new { jobId }, ApiResponse<JobScheduleResponse>.Success(schedule, "Schedule added."));
    }

    [HttpGet("{jobId:guid}/skills")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<JobSkillResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSkills(Guid jobId, CancellationToken cancellationToken)
    {
        var skills = await jobQueryService.GetSkillsAsync(jobId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<JobSkillResponse>>.Success(skills));
    }

    [HttpPost("{jobId:guid}/skills")]
    [ProducesResponseType(typeof(ApiResponse<JobSkillResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<JobSkillResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSkill(Guid jobId, [FromBody] AddJobSkillRequest request, CancellationToken cancellationToken)
    {
        var skill = await jobCommandService.AddSkillAsync(jobId, request, cancellationToken);

        if (skill is null)
        {
            return NotFound(ApiResponse<JobSkillResponse>.Failure("Job not found."));
        }

        return CreatedAtAction(nameof(GetSkills), new { jobId }, ApiResponse<JobSkillResponse>.Success(skill, "Skill requirement added."));
    }
}
