using PTimeJobs.Application.Common.Interfaces;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Services;

public sealed class DatabaseConnectionChecker(ApplicationDbContext dbContext) : IDatabaseConnectionChecker
{
    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Database.CanConnectAsync(cancellationToken);
    }
}
