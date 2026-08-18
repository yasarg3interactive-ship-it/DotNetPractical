using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Reviews.Dtos;
using PTimeJobs.Application.Reviews.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class ReviewsController(IReviewsService reviewsService) : ControllerBase
{
    [HttpGet("{reviewId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ReviewResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid reviewId, CancellationToken cancellationToken)
    {
        var review = await reviewsService.GetByIdAsync(reviewId, cancellationToken);

        if (review is null)
        {
            return NotFound(ApiResponse<ReviewResponse>.Failure("Review not found."));
        }

        return Ok(ApiResponse<ReviewResponse>.Success(review));
    }

    [HttpGet("for-entity")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ReviewResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetForEntity(
        [FromQuery] string targetEntityType,
        [FromQuery] Guid targetEntityId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var reviews = await reviewsService.GetForEntityAsync(targetEntityType, targetEntityId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ReviewResponse>>.Success(reviews));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ReviewResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var review = await reviewsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { reviewId = review.ReviewId },
            ApiResponse<ReviewResponse>.Success(review, "Review created."));
    }

    [HttpPatch("{reviewId:guid}/flag")]
    [ProducesResponseType(typeof(ApiResponse<ReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ReviewResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Flag(Guid reviewId, CancellationToken cancellationToken)
    {
        var review = await reviewsService.FlagAsync(reviewId, cancellationToken);

        if (review is null)
        {
            return NotFound(ApiResponse<ReviewResponse>.Failure("Review not found."));
        }

        return Ok(ApiResponse<ReviewResponse>.Success(review, "Review flagged."));
    }

    [HttpPatch("{reviewId:guid}/hide")]
    [ProducesResponseType(typeof(ApiResponse<ReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ReviewResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Hide(Guid reviewId, CancellationToken cancellationToken)
    {
        var review = await reviewsService.HideAsync(reviewId, cancellationToken);

        if (review is null)
        {
            return NotFound(ApiResponse<ReviewResponse>.Failure("Review not found."));
        }

        return Ok(ApiResponse<ReviewResponse>.Success(review, "Review hidden."));
    }
}
