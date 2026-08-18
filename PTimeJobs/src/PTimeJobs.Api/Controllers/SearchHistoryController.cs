using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Analytics.Dtos;
using PTimeJobs.Application.Analytics.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class SearchHistoryController(IPersonalizationService personalizationService) : ControllerBase
{
    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<SearchHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var searches = await personalizationService.GetSearchHistoryByUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<SearchHistoryResponse>>.Success(searches));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SearchHistoryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSearchHistoryRequest request, CancellationToken cancellationToken)
    {
        var search = await personalizationService.RecordSearchAsync(request, cancellationToken);
        return Created(string.Empty, ApiResponse<SearchHistoryResponse>.Success(search, "Search recorded."));
    }
}
