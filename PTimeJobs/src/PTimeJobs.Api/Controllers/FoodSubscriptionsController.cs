using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Food.Dtos;
using PTimeJobs.Application.Food.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class FoodSubscriptionsController(IFoodSubscriptionsService foodSubscriptionsService) : ControllerBase
{
    [HttpGet("{foodSubscriptionId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FoodSubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FoodSubscriptionResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid foodSubscriptionId, CancellationToken cancellationToken)
    {
        var subscription = await foodSubscriptionsService.GetByIdAsync(foodSubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return NotFound(ApiResponse<FoodSubscriptionResponse>.Failure("Food subscription not found."));
        }

        return Ok(ApiResponse<FoodSubscriptionResponse>.Success(subscription));
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<FoodSubscriptionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var subscriptions = await foodSubscriptionsService.GetByUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<FoodSubscriptionResponse>>.Success(subscriptions));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FoodSubscriptionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFoodSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var subscription = await foodSubscriptionsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { foodSubscriptionId = subscription.FoodSubscriptionId },
            ApiResponse<FoodSubscriptionResponse>.Success(subscription, "Subscription created."));
    }

    [HttpPatch("{foodSubscriptionId:guid}/activate")]
    [ProducesResponseType(typeof(ApiResponse<FoodSubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FoodSubscriptionResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid foodSubscriptionId, CancellationToken cancellationToken)
    {
        var subscription = await foodSubscriptionsService.ActivateAsync(foodSubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return NotFound(ApiResponse<FoodSubscriptionResponse>.Failure("Food subscription not found."));
        }

        return Ok(ApiResponse<FoodSubscriptionResponse>.Success(subscription, "Subscription activated."));
    }

    [HttpPatch("{foodSubscriptionId:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<FoodSubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FoodSubscriptionResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        Guid foodSubscriptionId,
        [FromBody] CancelFoodSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var subscription = await foodSubscriptionsService.CancelAsync(foodSubscriptionId, request.EndDate, cancellationToken);

        if (subscription is null)
        {
            return NotFound(ApiResponse<FoodSubscriptionResponse>.Failure("Food subscription not found."));
        }

        return Ok(ApiResponse<FoodSubscriptionResponse>.Success(subscription, "Subscription cancelled."));
    }
}
