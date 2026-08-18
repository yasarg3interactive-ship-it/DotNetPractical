using PTimeJobs.Application.Billing.Dtos;

namespace PTimeJobs.Application.Billing.Interfaces;

public interface IInvoicesService
{
    Task<InvoiceResponse?> GetByIdAsync(Guid invoiceId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<InvoiceResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<InvoiceResponse> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default);

    Task<InvoiceResponse?> MarkPaidAsync(Guid invoiceId, CancellationToken cancellationToken = default);
}
