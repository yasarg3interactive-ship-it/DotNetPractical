using PTimeJobs.Application.Billing.Dtos;

namespace PTimeJobs.Application.Billing.Interfaces;

public interface IPaymentsService
{
    Task<PaymentResponse?> GetByIdAsync(Guid paymentId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PaymentResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<PaymentResponse> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default);

    Task<PaymentResponse?> MarkPaidAsync(Guid paymentId, CancellationToken cancellationToken = default);

    Task<PaymentResponse?> MarkFailedAsync(Guid paymentId, CancellationToken cancellationToken = default);

    Task<PaymentResponse?> AddTransactionAsync(Guid paymentId, CreateTransactionRequest request, CancellationToken cancellationToken = default);
}
