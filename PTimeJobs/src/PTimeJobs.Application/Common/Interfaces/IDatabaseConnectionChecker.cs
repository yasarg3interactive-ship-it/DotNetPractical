namespace PTimeJobs.Application.Common.Interfaces;

public interface IDatabaseConnectionChecker
{
    Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);
}
