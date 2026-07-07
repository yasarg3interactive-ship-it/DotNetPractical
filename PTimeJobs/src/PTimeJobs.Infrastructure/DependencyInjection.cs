using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTimeJobs.Application.Common.Interfaces;
using PTimeJobs.Infrastructure.Persistence;
using PTimeJobs.Infrastructure.Persistence.Repositories;
using PTimeJobs.Infrastructure.Services;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Infrastructure.Users;

namespace PTimeJobs.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IDatabaseConnectionChecker, DatabaseConnectionChecker>();
        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
