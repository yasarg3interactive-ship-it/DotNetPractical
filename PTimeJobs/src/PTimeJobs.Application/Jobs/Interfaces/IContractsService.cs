using PTimeJobs.Application.Jobs.Dtos;

namespace PTimeJobs.Application.Jobs.Interfaces;

public interface IContractsService
{
    Task<ContractResponse?> GetByIdAsync(Guid contractId, CancellationToken cancellationToken = default);

    Task<ContractResponse> CreateAsync(CreateContractRequest request, CancellationToken cancellationToken = default);

    Task<ContractResponse?> ActivateAsync(Guid contractId, CancellationToken cancellationToken = default);

    Task<ContractResponse?> CompleteAsync(Guid contractId, EndContractRequest request, CancellationToken cancellationToken = default);

    Task<ContractResponse?> CancelAsync(Guid contractId, CancellationToken cancellationToken = default);

    Task<ContractResponse?> TerminateAsync(Guid contractId, EndContractRequest request, CancellationToken cancellationToken = default);
}
