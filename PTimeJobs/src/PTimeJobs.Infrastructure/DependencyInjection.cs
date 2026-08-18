using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PTimeJobs.Application.Common.Interfaces;
using PTimeJobs.Infrastructure.Persistence;
using PTimeJobs.Infrastructure.Persistence.Repositories;
using PTimeJobs.Infrastructure.Services;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Domain.Users;
using PTimeJobs.Infrastructure.Users;

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
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
