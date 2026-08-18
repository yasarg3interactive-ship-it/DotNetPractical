using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Jobs;

public sealed class ContractsService(ApplicationDbContext dbContext) : IContractsService
{
    public async Task<ContractResponse?> GetByIdAsync(Guid contractId, CancellationToken cancellationToken = default)
    {
        var contract = await dbContext.Contracts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);

        return contract is null ? null : ToResponse(contract);
    }

    public async Task<ContractResponse> CreateAsync(CreateContractRequest request, CancellationToken cancellationToken = default)
    {
        var jobExists = await dbContext.Jobs.AsNoTracking().AnyAsync(job => job.JobId == request.JobId, cancellationToken);
        if (!jobExists)
        {
            throw new InvalidOperationException("Job not found.");
        }

        var workerExists = await dbContext.WorkerProfiles
            .AsNoTracking()
            .AnyAsync(worker => worker.WorkerProfileId == request.WorkerProfileId, cancellationToken);

        if (!workerExists)
        {
            throw new InvalidOperationException("Worker profile not found.");
        }

        var employerExists = await dbContext.EmployerProfiles
            .AsNoTracking()
            .AnyAsync(employer => employer.EmployerProfileId == request.EmployerProfileId, cancellationToken);

        if (!employerExists)
        {
            throw new InvalidOperationException("Employer profile not found.");
        }

        SalaryModel? salaryModel = null;
        if (!string.IsNullOrWhiteSpace(request.SalaryModel))
        {
            if (!Enum.TryParse<SalaryModel>(request.SalaryModel, true, out var parsed))
            {
                throw new InvalidOperationException($"Unknown salary model '{request.SalaryModel}'.");
            }

            salaryModel = parsed;
        }

        var contract = Contract.Create(
            request.JobId,
            request.WorkerProfileId,
            request.EmployerProfileId,
            request.StartDate,
            request.ApplicationId,
            request.AgreedSalary,
            salaryModel,
            request.TermsUrl);

        dbContext.Contracts.Add(contract);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(contract);
    }

    public async Task<ContractResponse?> ActivateAsync(Guid contractId, CancellationToken cancellationToken = default)
    {
        var contract = await dbContext.Contracts.FirstOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);
        if (contract is null)
        {
            return null;
        }

        contract.Activate();
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(contract);
    }

    public async Task<ContractResponse?> CompleteAsync(Guid contractId, EndContractRequest request, CancellationToken cancellationToken = default)
    {
        var contract = await dbContext.Contracts.FirstOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);
        if (contract is null)
        {
            return null;
        }

        contract.Complete(request.EndDate);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(contract);
    }

    public async Task<ContractResponse?> CancelAsync(Guid contractId, CancellationToken cancellationToken = default)
    {
        var contract = await dbContext.Contracts.FirstOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);
        if (contract is null)
        {
            return null;
        }

        contract.Cancel();
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(contract);
    }

    public async Task<ContractResponse?> TerminateAsync(Guid contractId, EndContractRequest request, CancellationToken cancellationToken = default)
    {
        var contract = await dbContext.Contracts.FirstOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);
        if (contract is null)
        {
            return null;
        }

        contract.Terminate(request.EndDate);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(contract);
    }

    private static ContractResponse ToResponse(Contract contract) => new(
        contract.ContractId,
        contract.JobId,
        contract.ApplicationId,
        contract.WorkerProfileId,
        contract.EmployerProfileId,
        contract.Status.ToString(),
        contract.StartDate,
        contract.EndDate,
        contract.AgreedSalary,
        contract.SalaryModel?.ToString(),
        contract.TermsUrl,
        contract.CreatedAt);
}
