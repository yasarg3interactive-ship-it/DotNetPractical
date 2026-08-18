namespace PTimeJobs.Domain.Jobs;

public sealed class Contract
{
    private Contract()
    {
    }

    public Guid ContractId { get; private set; }
    public Guid JobId { get; private set; }
    public Guid? ApplicationId { get; private set; }
    public Guid WorkerProfileId { get; private set; }
    public Guid EmployerProfileId { get; private set; }
    public ContractStatus Status { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public decimal? AgreedSalary { get; private set; }
    public SalaryModel? SalaryModel { get; private set; }
    public string? TermsUrl { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static Contract Create(
        Guid jobId,
        Guid workerProfileId,
        Guid employerProfileId,
        DateOnly startDate,
        Guid? applicationId = null,
        decimal? agreedSalary = null,
        SalaryModel? salaryModel = null,
        string? termsUrl = null)
    {
        return new Contract
        {
            ContractId = Guid.NewGuid(),
            JobId = jobId,
            ApplicationId = applicationId,
            WorkerProfileId = workerProfileId,
            EmployerProfileId = employerProfileId,
            Status = ContractStatus.Draft,
            StartDate = startDate,
            AgreedSalary = agreedSalary,
            SalaryModel = salaryModel,
            TermsUrl = termsUrl,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Activate()
    {
        Status = ContractStatus.Active;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Complete(DateOnly endDate)
    {
        Status = ContractStatus.Completed;
        EndDate = endDate;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        Status = ContractStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Terminate(DateOnly endDate)
    {
        Status = ContractStatus.Terminated;
        EndDate = endDate;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
