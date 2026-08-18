namespace PTimeJobs.Application.Jobs.Dtos;

public sealed record ContractResponse(
    Guid ContractId,
    Guid JobId,
    Guid? ApplicationId,
    Guid WorkerProfileId,
    Guid EmployerProfileId,
    string Status,
    DateOnly StartDate,
    DateOnly? EndDate,
    decimal? AgreedSalary,
    string? SalaryModel,
    string? TermsUrl,
    DateTimeOffset CreatedAt);

public sealed record CreateContractRequest(
    Guid JobId,
    Guid WorkerProfileId,
    Guid EmployerProfileId,
    DateOnly StartDate,
    Guid? ApplicationId,
    decimal? AgreedSalary,
    string? SalaryModel,
    string? TermsUrl);

public sealed record EndContractRequest(DateOnly EndDate);
