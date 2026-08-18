namespace PTimeJobs.Domain.Billing;

public sealed class Transaction
{
    private Transaction()
    {
    }

    public Guid TransactionId { get; private set; }
    public Guid PaymentId { get; private set; }
    public string TransactionType { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? ProviderTransactionId { get; private set; }
    public string ProviderResponse { get; private set; } = "{}";
    public DateTimeOffset CreatedAt { get; private set; }

    public static Transaction Create(
        Guid paymentId,
        string transactionType,
        decimal amount,
        PaymentStatus status,
        string? providerTransactionId = null)
    {
        if (string.IsNullOrWhiteSpace(transactionType))
        {
            throw new InvalidOperationException("Transaction type is required.");
        }

        return new Transaction
        {
            TransactionId = Guid.NewGuid(),
            PaymentId = paymentId,
            TransactionType = transactionType,
            Amount = amount,
            Status = status,
            ProviderTransactionId = providerTransactionId,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
