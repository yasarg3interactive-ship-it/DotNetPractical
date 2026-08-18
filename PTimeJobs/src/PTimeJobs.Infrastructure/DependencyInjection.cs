using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PTimeJobs.Application.Common.Interfaces;
using PTimeJobs.Infrastructure.Persistence;
using PTimeJobs.Infrastructure.Persistence.Repositories;
using PTimeJobs.Infrastructure.Services;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Application.Analytics.Interfaces;
using PTimeJobs.Application.Billing.Interfaces;
using PTimeJobs.Application.Complaints.Interfaces;
using PTimeJobs.Application.Employers.Interfaces;
using PTimeJobs.Application.Food.Interfaces;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Application.Locations.Interfaces;
using PTimeJobs.Application.Messaging.Interfaces;
using PTimeJobs.Application.Notifications.Interfaces;
using PTimeJobs.Application.Reports.Interfaces;
using PTimeJobs.Application.Reviews.Interfaces;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Application.Workers.Interfaces;
using PTimeJobs.Domain.Users;
using PTimeJobs.Infrastructure.Accommodation;
using PTimeJobs.Infrastructure.Analytics;
using PTimeJobs.Infrastructure.Billing;
using PTimeJobs.Infrastructure.Complaints;
using PTimeJobs.Infrastructure.Employers;
using PTimeJobs.Infrastructure.Food;
using PTimeJobs.Infrastructure.Jobs;
using PTimeJobs.Infrastructure.Locations;
using PTimeJobs.Infrastructure.Messaging;
using PTimeJobs.Infrastructure.Notifications;
using PTimeJobs.Infrastructure.Reports;
using PTimeJobs.Infrastructure.Reviews;
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
        services.AddScoped<IJobCategoriesService, JobCategoriesService>();
        services.AddScoped<IMatchingScoresService, MatchingScoresService>();
        services.AddScoped<IContractsService, ContractsService>();
        services.AddScoped<IConversationsService, ConversationsService>();
        services.AddScoped<IMessagesService, MessagesService>();
        services.AddScoped<INotificationsService, NotificationsService>();
        services.AddScoped<IReviewsService, ReviewsService>();
        services.AddScoped<IComplaintsService, ComplaintsService>();
        services.AddScoped<IReportsService, ReportsService>();
        services.AddScoped<IVerificationsService, VerificationsService>();
        services.AddScoped<IRbacService, RbacService>();
        services.AddScoped<IFoodProvidersService, FoodProvidersService>();
        services.AddScoped<IFoodCatalogService, FoodCatalogService>();
        services.AddScoped<IFoodSubscriptionsService, FoodSubscriptionsService>();
        services.AddScoped<IAccommodationProvidersService, AccommodationProvidersService>();
        services.AddScoped<IFacilitiesAndRoomTypesService, FacilitiesAndRoomTypesService>();
        services.AddScoped<IPropertiesService, PropertiesService>();
        services.AddScoped<IRoomsService, RoomsService>();
        services.AddScoped<IAccommodationBookingsService, AccommodationBookingsService>();
        services.AddScoped<IBillingSubscriptionsService, BillingSubscriptionsService>();
        services.AddScoped<IInvoicesService, InvoicesService>();
        services.AddScoped<IPaymentsService, PaymentsService>();
        services.AddScoped<IAnalyticsEventsService, AnalyticsEventsService>();
        services.AddScoped<IPersonalizationService, PersonalizationService>();
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
