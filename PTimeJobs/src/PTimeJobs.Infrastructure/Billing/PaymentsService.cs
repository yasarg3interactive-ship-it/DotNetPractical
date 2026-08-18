using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Billing.Dtos;
using PTimeJobs.Application.Billing.Interfaces;
using PTimeJobs.Domain.Billing;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Billing;

public sealed class PaymentsService(ApplicationDbContext dbContext) : IPaymentsService
{
    public async Task<PaymentResponse?> GetByIdAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {
        var payment = await dbContext.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.PaymentId == paymentId, cancellationToken);
        return payment is null ? null : await BuildResponseAsync(payment, cancellationToken);
    }

    public async Task<IReadOnlyCollection<PaymentResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var payments = await dbContext.Payments
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        var responses = new List<PaymentResponse>();
        foreach (var payment in payments)
        {
            responses.Add(await BuildResponseAsync(payment, cancellationToken));
        }

        return responses;
    }

    public async Task<PaymentResponse> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        if (request.InvoiceId.HasValue)
        {
            var invoiceExists = await dbContext.Invoices.AsNoTracking().AnyAsync(i => i.InvoiceId == request.InvoiceId, cancellationToken);
            if (!invoiceExists)
            {
                throw new InvalidOperationException("Invoice not found.");
            }
        }

        var payment = Payment.Create(
            request.UserId,
            request.PayableEntityType,
            request.PayableEntityId,
            request.Amount,
            request.InvoiceId,
            request.Currency,
            request.PaymentMethod,
            request.ProviderName,
            request.ProviderPaymentId);

        dbContext.Payments.Add(payment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await BuildResponseAsync(payment, cancellationToken);
    }

    public async Task<PaymentResponse?> MarkPaidAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {
        var payment = await dbContext.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId, cancellationToken);
        if (payment is null)
        {
            return null;
        }

        payment.MarkPaid();
        await dbContext.SaveChangesAsync(cancellationToken);
        return await BuildResponseAsync(payment, cancellationToken);
    }

    public async Task<PaymentResponse?> MarkFailedAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {
        var payment = await dbContext.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId, cancellationToken);
        if (payment is null)
        {
            return null;
        }

        payment.MarkFailed();
        await dbContext.SaveChangesAsync(cancellationToken);
        return await BuildResponseAsync(payment, cancellationToken);
    }

    public async Task<PaymentResponse?> AddTransactionAsync(Guid paymentId, CreateTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var payment = await dbContext.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.PaymentId == paymentId, cancellationToken);
        if (payment is null)
        {
            return null;
        }

        if (!Enum.TryParse<PaymentStatus>(request.Status, true, out var status))
        {
            throw new InvalidOperationException($"Unknown payment status '{request.Status}'.");
        }

        var transaction = Transaction.Create(paymentId, request.TransactionType, request.Amount, status, request.ProviderTransactionId);
        dbContext.Transactions.Add(transaction);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await BuildResponseAsync(payment, cancellationToken);
    }

    private async Task<PaymentResponse> BuildResponseAsync(Payment payment, CancellationToken cancellationToken)
    {
        var transactions = await dbContext.Transactions
            .AsNoTracking()
            .Where(t => t.PaymentId == payment.PaymentId)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionResponse(t.TransactionId, t.PaymentId, t.TransactionType, t.Amount, t.Status.ToString(), t.ProviderTransactionId, t.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PaymentResponse(
            payment.PaymentId,
            payment.UserId,
            payment.InvoiceId,
            payment.PayableEntityType,
            payment.PayableEntityId,
            payment.Currency,
            payment.Amount,
            payment.Status.ToString(),
            payment.PaymentMethod,
            payment.CreatedAt,
            payment.PaidAt,
            transactions);
    }
}
