namespace PTimeJobs.Application.Billing.Dtos;

public sealed record BillingSubscriptionResponse(
    Guid BillingSubscriptionId,
    Guid UserId,
    string PlanCode,
    string Status,
    DateTimeOffset StartsAt,
    DateTimeOffset? EndsAt,
    string? ProviderName,
    DateTimeOffset CreatedAt);

public sealed record CreateBillingSubscriptionRequest(Guid UserId, string PlanCode, DateTimeOffset StartsAt, string? ProviderName, string? ProviderSubscriptionId);

public sealed record InvoiceResponse(
    Guid InvoiceId,
    Guid UserId,
    string InvoiceNumber,
    string Currency,
    decimal SubtotalAmount,
    decimal TaxAmount,
    decimal TotalAmount,
    string Status,
    DateTimeOffset IssuedAt,
    DateTimeOffset? DueAt,
    DateTimeOffset? PaidAt);

public sealed record CreateInvoiceRequest(Guid UserId, string InvoiceNumber, decimal SubtotalAmount, decimal TaxAmount, string Currency, DateTimeOffset? DueAt);

public sealed record TransactionResponse(
    Guid TransactionId,
    Guid PaymentId,
    string TransactionType,
    decimal Amount,
    string Status,
    string? ProviderTransactionId,
    DateTimeOffset CreatedAt);

public sealed record CreateTransactionRequest(string TransactionType, decimal Amount, string Status, string? ProviderTransactionId);

public sealed record PaymentResponse(
    Guid PaymentId,
    Guid UserId,
    Guid? InvoiceId,
    string PayableEntityType,
    Guid PayableEntityId,
    string Currency,
    decimal Amount,
    string Status,
    string? PaymentMethod,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PaidAt,
    IReadOnlyCollection<TransactionResponse> Transactions);

public sealed record CreatePaymentRequest(
    Guid UserId,
    string PayableEntityType,
    Guid PayableEntityId,
    decimal Amount,
    Guid? InvoiceId,
    string Currency,
    string? PaymentMethod,
    string? ProviderName,
    string? ProviderPaymentId);
