using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class MatchingScoresController(IMatchingScoresService matchingScoresService) : ControllerBase
{
    [HttpGet("by-job/{jobId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<MatchingScoreResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByJob(Guid jobId, [FromQuery] int top = 20, CancellationToken cancellationToken = default)
    {
        var scores = await matchingScoresService.GetByJobAsync(jobId, top, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<MatchingScoreResponse>>.Success(scores));
    }

    [HttpGet("by-worker/{workerProfileId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<MatchingScoreResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByWorker(Guid workerProfileId, [FromQuery] int top = 20, CancellationToken cancellationToken = default)
    {
        var scores = await matchingScoresService.GetByWorkerAsync(workerProfileId, top, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<MatchingScoreResponse>>.Success(scores));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MatchingScoreResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMatchingScoreRequest request, CancellationToken cancellationToken)
    {
        var score = await matchingScoresService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByJob), new { jobId = score.JobId }, ApiResponse<MatchingScoreResponse>.Success(score, "Matching score recorded."));
    }
}
