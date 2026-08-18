using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PTimeJobs.Application.Common.Interfaces;
using PTimeJobs.Infrastructure.Persistence;
using PTimeJobs.Infrastructure.Persistence.Repositories;
using PTimeJobs.Infrastructure.Services;
using PTimeJobs.Application.Employers.Interfaces;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Application.Locations.Interfaces;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Application.Workers.Interfaces;
using PTimeJobs.Domain.Users;
using PTimeJobs.Infrastructure.Employers;
using PTimeJobs.Infrastructure.Jobs;
using PTimeJobs.Infrastructure.Locations;
using PTimeJobs.Infrastructure.Users;
using PTimeJobs.Infrastructure.Workers;

namespace PTimeJobs.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Map .NET enums to their matching Postgres native enum types so EF Core
        // can read/write these columns without "column is of type X but expression
        // is of type text" errors.
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<AccountStatus>("account_status");
        dataSourceBuilder.MapEnum<SessionStatus>("session_status");
        dataSourceBuilder.MapEnum<VerificationChannel>("verification_channel");
        dataSourceBuilder.MapEnum<VerificationStatus>("verification_status");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Jobs.EmploymentType>("employment_type");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Jobs.SalaryModel>("salary_model");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Jobs.JobStatus>("job_status");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Jobs.ApplicationStatus>("application_status");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Messaging.ConversationType>("conversation_type");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Notifications.NotificationStatus>("notification_status");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Reviews.ReviewStatus>("review_status");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Complaints.ComplaintStatus>("complaint_status");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Billing.SubscriptionStatus>("subscription_status");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Billing.PaymentStatus>("payment_status");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Accommodation.BookingStatus>("booking_status");
        dataSourceBuilder.MapEnum<PTimeJobs.Domain.Jobs.ContractStatus>("contract_status");
        var dataSource = dataSourceBuilder.Build();

        services.AddSingleton(dataSource);
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(dataSource));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IDatabaseConnectionChecker, DatabaseConnectionChecker>();
        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IJobQueryService, JobQueryService>();
        services.AddScoped<IJobCommandService, JobCommandService>();
        services.AddScoped<IJobApplicationQueryService, JobApplicationQueryService>();
        services.AddScoped<IJobApplicationCommandService, JobApplicationCommandService>();
        services.AddScoped<ILocationsQueryService, LocationsQueryService>();
        services.AddScoped<ILocationsCommandService, LocationsCommandService>();
        services.AddScoped<ISkillsService, SkillsService>();
        services.AddScoped<IWorkerProfileQueryService, WorkerProfileQueryService>();
        services.AddScoped<IWorkerProfileCommandService, WorkerProfileCommandService>();
        services.AddScoped<IEmployerProfileQueryService, EmployerProfileQueryService>();
        services.AddScoped<IEmployerProfileCommandService, EmployerProfileCommandService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
