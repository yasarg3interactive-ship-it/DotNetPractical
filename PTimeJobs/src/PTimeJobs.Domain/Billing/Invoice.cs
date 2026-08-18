namespace PTimeJobs.Domain.Billing;

public sealed class Invoice
{
    private Invoice()
    {
    }

    public Guid InvoiceId { get; private set; }
    public Guid UserId { get; private set; }
    public string InvoiceNumber { get; private set; } = string.Empty;
    public string Currency { get; private set; } = "INR";
    public decimal SubtotalAmount { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTimeOffset IssuedAt { get; private set; }
    public DateTimeOffset? DueAt { get; private set; }
    public DateTimeOffset? PaidAt { get; private set; }
    public string Metadata { get; private set; } = "{}";

    public static Invoice Create(
        Guid userId,
        string invoiceNumber,
        decimal subtotalAmount,
        decimal taxAmount = 0m,
        string currency = "INR",
        DateTimeOffset? dueAt = null)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber))
        {
            throw new InvalidOperationException("Invoice number is required.");
        }

        if (subtotalAmount < 0 || taxAmount < 0)
        {
            throw new InvalidOperationException("Amounts cannot be negative.");
        }

        return new Invoice
        {
            InvoiceId = Guid.NewGuid(),
            UserId = userId,
            InvoiceNumber = invoiceNumber,
            Currency = currency,
            SubtotalAmount = subtotalAmount,
            TaxAmount = taxAmount,
            TotalAmount = subtotalAmount + taxAmount,
            Status = PaymentStatus.Pending,
            IssuedAt = DateTimeOffset.UtcNow,
            DueAt = dueAt
        };
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
