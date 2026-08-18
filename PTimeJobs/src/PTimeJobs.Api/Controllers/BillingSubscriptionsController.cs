using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Billing.Dtos;
using PTimeJobs.Application.Billing.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class BillingSubscriptionsController(IBillingSubscriptionsService billingSubscriptionsService) : ControllerBase
{
    [HttpGet("{billingSubscriptionId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BillingSubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BillingSubscriptionResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid billingSubscriptionId, CancellationToken cancellationToken)
    {
        var subscription = await billingSubscriptionsService.GetByIdAsync(billingSubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return NotFound(ApiResponse<BillingSubscriptionResponse>.Failure("Subscription not found."));
        }

        return Ok(ApiResponse<BillingSubscriptionResponse>.Success(subscription));
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<BillingSubscriptionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var subscriptions = await billingSubscriptionsService.GetByUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<BillingSubscriptionResponse>>.Success(subscriptions));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BillingSubscriptionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBillingSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var subscription = await billingSubscriptionsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { billingSubscriptionId = subscription.BillingSubscriptionId },
            ApiResponse<BillingSubscriptionResponse>.Success(subscription, "Subscription created."));
    }

    [HttpPatch("{billingSubscriptionId:guid}/activate")]
    [ProducesResponseType(typeof(ApiResponse<BillingSubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BillingSubscriptionResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid billingSubscriptionId, CancellationToken cancellationToken)
    {
        var subscription = await billingSubscriptionsService.ActivateAsync(billingSubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return NotFound(ApiResponse<BillingSubscriptionResponse>.Failure("Subscription not found."));
        }

        return Ok(ApiResponse<BillingSubscriptionResponse>.Success(subscription, "Subscription activated."));
    }

    [HttpPatch("{billingSubscriptionId:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<BillingSubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BillingSubscriptionResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        Guid billingSubscriptionId,
        [FromBody] CancelBillingSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var subscription = await billingSubscriptionsService.CancelAsync(billingSubscriptionId, request.EndsAt, cancellationToken);

        if (subscription is null)
        {
            return NotFound(ApiResponse<BillingSubscriptionResponse>.Failure("Subscription not found."));
        }

        return Ok(ApiResponse<BillingSubscriptionResponse>.Success(subscription, "Subscription cancelled."));
    }
}
