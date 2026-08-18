using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Billing.Dtos;
using PTimeJobs.Application.Billing.Interfaces;
using PTimeJobs.Domain.Billing;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Billing;

public sealed class InvoicesService(ApplicationDbContext dbContext) : IInvoicesService
{
    public async Task<InvoiceResponse?> GetByIdAsync(Guid invoiceId, CancellationToken cancellationToken = default)
    {
        var invoice = await dbContext.Invoices.AsNoTracking().FirstOrDefaultAsync(i => i.InvoiceId == invoiceId, cancellationToken);
        return invoice is null ? null : ToResponse(invoice);
    }

    public async Task<IReadOnlyCollection<InvoiceResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var invoices = await dbContext.Invoices
            .AsNoTracking()
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.IssuedAt)
            .ToListAsync(cancellationToken);

        return invoices.Select(ToResponse).ToList();
    }

    public async Task<InvoiceResponse> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var numberTaken = await dbContext.Invoices
            .AsNoTracking()
            .AnyAsync(i => i.InvoiceNumber == request.InvoiceNumber, cancellationToken);

        if (numberTaken)
        {
            throw new InvalidOperationException("An invoice with this number already exists.");
        }

        var invoice = Invoice.Create(request.UserId, request.InvoiceNumber, request.SubtotalAmount, request.TaxAmount, request.Currency, request.DueAt);
        dbContext.Invoices.Add(invoice);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(invoice);
    }

    public async Task<InvoiceResponse?> MarkPaidAsync(Guid invoiceId, CancellationToken cancellationToken = default)
    {
        var invoice = await dbContext.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == invoiceId, cancellationToken);
        if (invoice is null)
        {
            return null;
        }

        invoice.MarkPaid();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(invoice);
    }

    private static InvoiceResponse ToResponse(Invoice invoice) => new(
        invoice.InvoiceId,
        invoice.UserId,
        invoice.InvoiceNumber,
        invoice.Currency,
        invoice.SubtotalAmount,
        invoice.TaxAmount,
        invoice.TotalAmount,
        invoice.Status.ToString(),
        invoice.IssuedAt,
        invoice.DueAt,
        invoice.PaidAt);
}
