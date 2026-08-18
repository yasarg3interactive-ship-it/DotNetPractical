namespace PTimeJobs.Domain.Billing;

public sealed class Payment
{
    private Payment()
    {
    }

    public Guid PaymentId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? InvoiceId { get; private set; }
    public string PayableEntityType { get; private set; } = string.Empty;
    public Guid PayableEntityId { get; private set; }
    public string Currency { get; private set; } = "INR";
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? PaymentMethod { get; private set; }
    public string? ProviderName { get; private set; }
    public string? ProviderPaymentId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? PaidAt { get; private set; }
    public string Metadata { get; private set; } = "{}";

    public static Payment Create(
        Guid userId,
        string payableEntityType,
        Guid payableEntityId,
        decimal amount,
        Guid? invoiceId = null,
        string currency = "INR",
        string? paymentMethod = null,
        string? providerName = null,
        string? providerPaymentId = null)
    {
        if (string.IsNullOrWhiteSpace(payableEntityType))
        {
            throw new InvalidOperationException("Payable entity type is required.");
        }

        if (amount <= 0)
        {
            throw new InvalidOperationException("Amount must be greater than zero.");
        }

        return new Payment
        {
            PaymentId = Guid.NewGuid(),
            UserId = userId,
            InvoiceId = invoiceId,
            PayableEntityType = payableEntityType,
            PayableEntityId = payableEntityId,
            Currency = currency,
            Amount = amount,
            Status = PaymentStatus.Pending,
            PaymentMethod = paymentMethod,
            ProviderName = providerName,
            ProviderPaymentId = providerPaymentId,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void MarkAuthorized()
    {
        Status = PaymentStatus.Authorized;
    }

    public void MarkPaid()
    {
        Status = PaymentStatus.Paid;
        PaidAt = DateTimeOffset.UtcNow;
    }

    public void MarkFailed()
    {
        Status = PaymentStatus.Failed;
    }

    public void MarkRefunded()
    {
        Status = PaymentStatus.Refunded;
    }

    public void Cancel()
    {
        Status = PaymentStatus.Cancelled;
    }
}
