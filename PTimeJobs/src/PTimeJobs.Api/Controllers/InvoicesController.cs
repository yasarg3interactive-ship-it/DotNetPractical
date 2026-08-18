using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Billing.Dtos;
using PTimeJobs.Application.Billing.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class InvoicesController(IInvoicesService invoicesService) : ControllerBase
{
    [HttpGet("{invoiceId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<InvoiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<InvoiceResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid invoiceId, CancellationToken cancellationToken)
    {
        var invoice = await invoicesService.GetByIdAsync(invoiceId, cancellationToken);

        if (invoice is null)
        {
            return NotFound(ApiResponse<InvoiceResponse>.Failure("Invoice not found."));
        }

        return Ok(ApiResponse<InvoiceResponse>.Success(invoice));
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<InvoiceResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var invoices = await invoicesService.GetByUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<InvoiceResponse>>.Success(invoices));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<InvoiceResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = await invoicesService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { invoiceId = invoice.InvoiceId },
            ApiResponse<InvoiceResponse>.Success(invoice, "Invoice created."));
    }

    [HttpPatch("{invoiceId:guid}/mark-paid")]
    [ProducesResponseType(typeof(ApiResponse<InvoiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<InvoiceResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkPaid(Guid invoiceId, CancellationToken cancellationToken)
    {
        var invoice = await invoicesService.MarkPaidAsync(invoiceId, cancellationToken);

        if (invoice is null)
        {
            return NotFound(ApiResponse<InvoiceResponse>.Failure("Invoice not found."));
        }

        return Ok(ApiResponse<InvoiceResponse>.Success(invoice, "Invoice marked paid."));
    }
}
