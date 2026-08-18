using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class JobCategoriesController(IJobCategoriesService jobCategoriesService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<JobCategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await jobCategoriesService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<JobCategoryResponse>>.Success(categories));
    }

    [HttpGet("{jobCategoryId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<JobCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<JobCategoryResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid jobCategoryId, CancellationToken cancellationToken)
    {
        var category = await jobCategoriesService.GetByIdAsync(jobCategoryId, cancellationToken);

        if (category is null)
        {
            return NotFound(ApiResponse<JobCategoryResponse>.Failure("Job category not found."));
        }

        return Ok(ApiResponse<JobCategoryResponse>.Success(category));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<JobCategoryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateJobCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await jobCategoriesService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { jobCategoryId = category.JobCategoryId },
            ApiResponse<JobCategoryResponse>.Success(category, "Job category created."));
    }
}
