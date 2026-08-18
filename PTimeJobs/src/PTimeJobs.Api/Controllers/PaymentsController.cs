using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Billing.Dtos;
using PTimeJobs.Application.Billing.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class PaymentsController(IPaymentsService paymentsService) : ControllerBase
{
    [HttpGet("{paymentId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid paymentId, CancellationToken cancellationToken)
    {
        var payment = await paymentsService.GetByIdAsync(paymentId, cancellationToken);

        if (payment is null)
        {
            return NotFound(ApiResponse<PaymentResponse>.Failure("Payment not found."));
        }

        return Ok(ApiResponse<PaymentResponse>.Success(payment));
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<PaymentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var payments = await paymentsService.GetByUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<PaymentResponse>>.Success(payments));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        var payment = await paymentsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { paymentId = payment.PaymentId },
            ApiResponse<PaymentResponse>.Success(payment, "Payment created."));
    }

    [HttpPatch("{paymentId:guid}/mark-paid")]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkPaid(Guid paymentId, CancellationToken cancellationToken)
    {
        var payment = await paymentsService.MarkPaidAsync(paymentId, cancellationToken);

        if (payment is null)
        {
            return NotFound(ApiResponse<PaymentResponse>.Failure("Payment not found."));
        }

        return Ok(ApiResponse<PaymentResponse>.Success(payment, "Payment marked paid."));
    }

    [HttpPatch("{paymentId:guid}/mark-failed")]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkFailed(Guid paymentId, CancellationToken cancellationToken)
    {
        var payment = await paymentsService.MarkFailedAsync(paymentId, cancellationToken);

        if (payment is null)
        {
            return NotFound(ApiResponse<PaymentResponse>.Failure("Payment not found."));
        }

        return Ok(ApiResponse<PaymentResponse>.Success(payment, "Payment marked failed."));
    }

    [HttpPost("{paymentId:guid}/transactions")]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTransaction(Guid paymentId, [FromBody] CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var payment = await paymentsService.AddTransactionAsync(paymentId, request, cancellationToken);

        if (payment is null)
        {
            return NotFound(ApiResponse<PaymentResponse>.Failure("Payment not found."));
        }

        return Ok(ApiResponse<PaymentResponse>.Success(payment, "Transaction recorded."));
    }
}
