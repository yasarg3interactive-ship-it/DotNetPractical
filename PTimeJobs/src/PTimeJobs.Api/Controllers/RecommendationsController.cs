using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Analytics.Dtos;
using PTimeJobs.Application.Analytics.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class RecommendationsController(IPersonalizationService personalizationService) : ControllerBase
{
    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<RecommendationHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var recommendations = await personalizationService.GetRecommendationsByUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<RecommendationHistoryResponse>>.Success(recommendations));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RecommendationHistoryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRecommendationRequest request, CancellationToken cancellationToken)
    {
        var recommendation = await personalizationService.CreateRecommendationAsync(request, cancellationToken);
        return Created(string.Empty, ApiResponse<RecommendationHistoryResponse>.Success(recommendation, "Recommendation recorded."));
    }

    [HttpPatch("{recommendationId:guid}/click")]
    [ProducesResponseType(typeof(ApiResponse<RecommendationHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<RecommendationHistoryResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RecordClick(Guid recommendationId, CancellationToken cancellationToken)
    {
        var recommendation = await personalizationService.RecordClickAsync(recommendationId, cancellationToken);

        if (recommendation is null)
        {
            return NotFound(ApiResponse<RecommendationHistoryResponse>.Failure("Recommendation not found."));
        }

        return Ok(ApiResponse<RecommendationHistoryResponse>.Success(recommendation, "Click recorded."));
    }

    [HttpPatch("{recommendationId:guid}/dismiss")]
    [ProducesResponseType(typeof(ApiResponse<RecommendationHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<RecommendationHistoryResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RecordDismissal(Guid recommendationId, CancellationToken cancellationToken)
    {
        var recommendation = await personalizationService.RecordDismissalAsync(recommendationId, cancellationToken);

        if (recommendation is null)
        {
            return NotFound(ApiResponse<RecommendationHistoryResponse>.Failure("Recommendation not found."));
        }

        return Ok(ApiResponse<RecommendationHistoryResponse>.Success(recommendation, "Dismissal recorded."));
    }

    [HttpPatch("{recommendationId:guid}/convert")]
    [ProducesResponseType(typeof(ApiResponse<RecommendationHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<RecommendationHistoryResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RecordConversion(Guid recommendationId, CancellationToken cancellationToken)
    {
        var recommendation = await personalizationService.RecordConversionAsync(recommendationId, cancellationToken);

        if (recommendation is null)
        {
            return NotFound(ApiResponse<RecommendationHistoryResponse>.Failure("Recommendation not found."));
        }

        return Ok(ApiResponse<RecommendationHistoryResponse>.Success(recommendation, "Conversion recorded."));
    }
}
