# PTimeJobs Project Documentation

Generated from source path: `D:\Practical Project`

## 1. Project Goal

PTimeJobs is a Part-Time Work and Living Marketplace Platform. The project goal is to connect workers, employers, accommodation providers, and food providers through one marketplace and operations console.

The planned platform covers:

- Worker identity, profile, skills, documents, availability, education, and experience.
- Employer profiles, job posting, schedules, required skills, applications, shortlists, hiring history, and contracts.
- AI-assisted worker-job matching and recommendation history.
- Accommodation provider listings, properties, rooms, room availability, facilities, images, and bookings.
- Food providers, food items, food plans, subscriptions, and delivery areas.
- Conversations, messages, attachments, and notifications.
- Billing subscriptions, invoices, payments, and payment transactions.
- Reviews, complaints, reports, audit logs, analytics events, search history, and behavior tracking.

The current codebase is a clean-architecture backend starter with a React/Vite operations dashboard. The domain model and database schema are broad, while the exposed API endpoints are still starter-level.

## 2. Technology Stack

Backend:

- ASP.NET Core 8 Web API
- C#
- Entity Framework Core 8
- Npgsql PostgreSQL provider
- JWT bearer authentication
- Swagger/OpenAPI
- Repository pattern and unit of work
- Clean Architecture / Domain Driven Design structure

Frontend:

- React
- TypeScript
- Vite
- lucide-react icons
- CSS-based operations dashboard UI

Database:

- PostgreSQL 15+
- `pgcrypto` for UUID generation support
- `citext` for case-insensitive text use cases
- Optional PostGIS for location search
- Optional pgvector for AI embeddings

Testing:

- xUnit test project
- Starter tests for the standard API response model

## 3. Solution Structure

```text
PTimeJobs/
  PTimeJobs.slnx
  src/
    PTimeJobs.Api/              ASP.NET Core API layer
    PTimeJobs.Application/      DTOs, interfaces, common models
    PTimeJobs.Domain/           Entities, enums, domain rules
    PTimeJobs.Infrastructure/   EF Core, repositories, services
    PTimeJobs.Web/              React/Vite frontend console
  tests/
    PTimeJobs.Tests/            Unit tests

database_migrations/            Existing SQL schema scripts
Database_Migration/             Folder reserved for future module migrations
Backend_Architecture_Guide.md   Backend implementation rules
Feature_Intake_Template.md      Feature planning template
```

## 4. Architecture Overview

The project follows Clean Architecture.

Domain layer:

- Contains entities and enums.
- Has no dependency on EF Core, controllers, external APIs, or infrastructure.
- Owns basic business rules such as validating required user identity fields or job salary ranges.

Application layer:

- Contains DTOs, interfaces, and shared response models.
- Defines contracts such as `IApplicationDbContext`, `IRepository<T>`, `IUnitOfWork`, `IDatabaseConnectionChecker`, and `IUserQueryService`.

Infrastructure layer:

- Implements persistence using EF Core and PostgreSQL.
- Contains `ApplicationDbContext`, entity configurations, repositories, unit of work, database connection checker, and query services.

API layer:

- Contains controllers, middleware, CORS, Swagger, authentication, and request routing.
- Uses standard API responses and should keep controllers thin.

Frontend layer:

- Provides a dashboard-style operations console.
- Calls backend health endpoint through `VITE_API_BASE_URL`.

## 5. Backend Startup Flow

Main file: `PTimeJobs/src/PTimeJobs.Api/Program.cs`

Key behavior:

- Registers controllers.
- Enables Swagger and bearer-token Swagger authorization.
- Registers application and infrastructure services.
- Enables CORS for `localhost:5173` and `https://localhost:5173`.
- Configures JWT authentication if `Jwt:Key` exists.
- Registers exception middleware.
- Maps API controllers.
- Redirects `/` to `/swagger` in development.

Code example:

```csharp
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

JWT configuration is conditional:

```csharp
var jwtKey = builder.Configuration["Jwt:Key"];
if (!string.IsNullOrWhiteSpace(jwtKey))
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey))
            };
        });
}
```

## 6. API Routing and Standard Response

The API uses a versioned route pattern through `ApiConstants.BaseRoute`.

Current visible endpoints:

- `GET /api/v1/health`
- `GET /api/v1/users/{userId}` with `admin` role authorization

Standard response model:

```csharp
public sealed record ApiResponse<T>(
    string Status,
    string Message,
    T? Data,
    IReadOnlyDictionary<string, string[]>? Errors = null)
{
    public static ApiResponse<T> Success(
        T? data,
        string message = "Operation completed successfully.")
    {
        return new ApiResponse<T>("success", message, data);
    }

    public static ApiResponse<T> Failure(
        string message,
        IReadOnlyDictionary<string, string[]>? errors = null)
    {
        return new ApiResponse<T>("error", message, default, errors);
    }
}
```

Success response example:

```json
{
  "status": "success",
  "message": "Service is healthy.",
  "data": {
    "service": "PTimeJobs API",
    "databaseConnected": true,
    "checkedAt": "2026-08-18T00:00:00Z"
  }
}
```

Error response example:

```json
{
  "status": "error",
  "message": "Database connection failed.",
  "data": null,
  "errors": null
}
```

## 7. Implemented API Endpoints

### Health Endpoint

File: `PTimeJobs/src/PTimeJobs.Api/Controllers/HealthController.cs`

Purpose:

- Checks API availability.
- Checks database connectivity through `IDatabaseConnectionChecker`.
- Returns HTTP 200 when healthy.
- Returns HTTP 503 when the database cannot connect.

Code example:

```csharp
[HttpGet]
public async Task<IActionResult> Get(CancellationToken cancellationToken)
{
    var databaseConnected =
        await databaseConnectionChecker.CanConnectAsync(cancellationToken);

    var data = new
    {
        service = "PTimeJobs API",
        databaseConnected,
        checkedAt = DateTimeOffset.UtcNow
    };

    if (!databaseConnected)
    {
        return StatusCode(
            StatusCodes.Status503ServiceUnavailable,
            ApiResponse<object>.Failure("Database connection failed."));
    }

    return Ok(ApiResponse<object>.Success(data, "Service is healthy."));
}
```

### User Summary Endpoint

File: `PTimeJobs/src/PTimeJobs.Api/Controllers/UsersController.cs`

Purpose:

- Allows an admin to fetch a user summary by ID.
- Returns user identity, status, verification flags, roles, and created date.
- Returns HTTP 404 when the user is not found.

Code example:

```csharp
[Authorize(Roles = "admin")]
[HttpGet("{userId:guid}")]
public async Task<IActionResult> GetById(
    Guid userId,
    CancellationToken cancellationToken)
{
    var user = await userQueryService.GetByIdAsync(userId, cancellationToken);

    if (user is null)
    {
        return NotFound(ApiResponse<UserSummaryResponse>.Failure("User not found."));
    }

    return Ok(ApiResponse<UserSummaryResponse>.Success(user));
}
```

Query service projection:

```csharp
return await dbContext.Users
    .AsNoTracking()
    .Where(user => user.UserId == userId)
    .Select(user => new UserSummaryResponse(
        user.UserId,
        user.Email,
        user.MobileNumber,
        user.Status.ToString(),
        user.IsEmailVerified,
        user.IsMobileVerified,
        user.UserRoles
            .Select(userRole => userRole.Role.RoleCode)
            .OrderBy(roleCode => roleCode)
            .ToArray(),
        user.CreatedAt))
    .FirstOrDefaultAsync(cancellationToken);
```

## 8. Dependency Injection and Persistence

Infrastructure registration file:

`PTimeJobs/src/PTimeJobs.Infrastructure/DependencyInjection.cs`

Important registrations:

- `NpgsqlDataSource` singleton
- EF Core `ApplicationDbContext`
- `IApplicationDbContext`
- `IDatabaseConnectionChecker`
- `IUserQueryService`
- Generic `IRepository<T>`
- `IUnitOfWork`

PostgreSQL enum mapping is configured both in Npgsql and EF Core model creation.

Example:

```csharp
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<AccountStatus>("account_status");
dataSourceBuilder.MapEnum<SessionStatus>("session_status");
dataSourceBuilder.MapEnum<VerificationChannel>("verification_channel");
dataSourceBuilder.MapEnum<VerificationStatus>("verification_status");
dataSourceBuilder.MapEnum<EmploymentType>("employment_type");
dataSourceBuilder.MapEnum<SalaryModel>("salary_model");
dataSourceBuilder.MapEnum<JobStatus>("job_status");
dataSourceBuilder.MapEnum<ApplicationStatus>("application_status");
dataSourceBuilder.MapEnum<ConversationType>("conversation_type");
dataSourceBuilder.MapEnum<NotificationStatus>("notification_status");
dataSourceBuilder.MapEnum<ReviewStatus>("review_status");
dataSourceBuilder.MapEnum<ComplaintStatus>("complaint_status");
dataSourceBuilder.MapEnum<SubscriptionStatus>("subscription_status");
dataSourceBuilder.MapEnum<PaymentStatus>("payment_status");
dataSourceBuilder.MapEnum<BookingStatus>("booking_status");
dataSourceBuilder.MapEnum<ContractStatus>("contract_status");
```

Generic repository:

```csharp
public class Repository<TEntity>(ApplicationDbContext dbContext)
    : IRepository<TEntity>
    where TEntity : class
{
    public async Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public async Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }
}
```

Unit of work:

```csharp
public sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
```

## 9. Entity Framework DbContext

File: `PTimeJobs/src/PTimeJobs.Infrastructure/Persistence/ApplicationDbContext.cs`

The DbContext exposes DbSets for all major modules:

- Users: `Users`, `Roles`, `UserRoles`, `UserProfiles`, `UserSessions`, `Verifications`, `Permissions`, `RolePermissions`
- Locations: `Countries`, `States`, `Cities`, `Areas`, `Locations`
- Workers: `WorkerProfiles`, `WorkerSkills`, `WorkerExperiences`, `WorkerEducations`, `WorkerDocuments`, `WorkerAvailabilities`
- Jobs: `Skills`, `EmployerProfiles`, `JobCategories`, `Jobs`, `JobLocations`, `JobSchedules`, `JobSkills`, `JobApplications`, `Shortlists`, `HiringStatusHistories`, `Contracts`, `MatchingScores`
- Accommodation: `AccommodationProviders`, `Properties`, `PropertyFacilities`, `PropertyImages`, `Facilities`, `RoomTypes`, `Rooms`, `RoomAvailabilities`, `AccommodationBookings`
- Food: `FoodProviders`, `FoodItems`, `FoodPlans`, `FoodPlanItems`, `FoodSubscriptions`, `DeliveryAreas`
- Messaging: `Conversations`, `ConversationParticipants`, `Messages`, `MessageAttachments`
- Notifications: `Notifications`
- Billing: `BillingSubscriptions`, `Invoices`, `Payments`, `Transactions`
- Trust and analytics: `Reviews`, `Complaints`, `Reports`, `AuditLogs`, `AnalyticsEvents`, `UserBehaviorEvents`, `SearchHistories`, `RecommendationHistories`, `UserPreferences`

Model creation:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.HasPostgresEnum<AccountStatus>();
    modelBuilder.HasPostgresEnum<SessionStatus>();
    modelBuilder.HasPostgresEnum<VerificationChannel>();
    modelBuilder.HasPostgresEnum<VerificationStatus>();
    modelBuilder.HasPostgresEnum<EmploymentType>();
    modelBuilder.HasPostgresEnum<SalaryModel>();
    modelBuilder.HasPostgresEnum<JobStatus>();
    modelBuilder.HasPostgresEnum<ApplicationStatus>();
    modelBuilder.HasPostgresEnum<ConversationType>();
    modelBuilder.HasPostgresEnum<NotificationStatus>();
    modelBuilder.HasPostgresEnum<ReviewStatus>();
    modelBuilder.HasPostgresEnum<ComplaintStatus>();
    modelBuilder.HasPostgresEnum<SubscriptionStatus>();
    modelBuilder.HasPostgresEnum<PaymentStatus>();
    modelBuilder.HasPostgresEnum<BookingStatus>();
    modelBuilder.HasPostgresEnum<ContractStatus>();

    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
}
```

## 10. Domain Model Details

### User

The user entity is the identity anchor for the full marketplace.

Important fields:

- `UserId`
- `Email`
- `MobileNumber`
- `PasswordHash`
- `Status`
- `IsEmailVerified`
- `IsMobileVerified`
- `LastLoginAt`
- `LastActiveAt`
- `FailedLoginCount`
- `LockedUntil`
- `Metadata`
- `CreatedAt`
- `UpdatedAt`
- `UserRoles`

Important behavior:

- Requires either email or mobile number.
- Requires password hash.
- Starts as `Pending`.
- Can mark email/mobile verified.
- Can activate or suspend account.
- Records successful and failed logins.
- Locks account for 15 minutes after 5 failed login attempts.

Code example:

```csharp
public static User Create(string? email, string? mobileNumber, string passwordHash)
{
    if (string.IsNullOrWhiteSpace(email) &&
        string.IsNullOrWhiteSpace(mobileNumber))
    {
        throw new InvalidOperationException("Email or mobile number is required.");
    }

    if (string.IsNullOrWhiteSpace(passwordHash))
    {
        throw new InvalidOperationException("Password hash is required.");
    }

    return new User
    {
        UserId = Guid.NewGuid(),
        Email = email,
        MobileNumber = mobileNumber,
        PasswordHash = passwordHash,
        Status = AccountStatus.Pending,
        CreatedAt = DateTimeOffset.UtcNow,
        UpdatedAt = DateTimeOffset.UtcNow
    };
}
```

### Worker Profile

Important fields:

- `WorkerProfileId`
- `UserId`
- `Headline`
- `ExpectedSalaryMin`
- `ExpectedSalaryMax`
- `ExpectedSalaryModel`
- `TotalExperienceMonths`
- `CurrentLocationId`
- `ResumeUrl`
- `ProfileStrengthScore`
- `AverageRating`
- `RatingCount`
- `MatchingMetadata`
- `Skills`

Important behavior:

- Creates a worker profile linked to a user.
- Updates headline, expected salary, and resume URL.
- Validates salary range.
- Maintains average rating and rating count.

Code example:

```csharp
public void UpdateExpectedSalary(decimal? min, decimal? max, SalaryModel? model)
{
    if (min.HasValue && max.HasValue && min > max)
    {
        throw new InvalidOperationException(
            "Minimum expected salary cannot exceed maximum.");
    }

    ExpectedSalaryMin = min;
    ExpectedSalaryMax = max;
    ExpectedSalaryModel = model;
    UpdatedAt = DateTimeOffset.UtcNow;
}
```

### Job

Important fields:

- `JobId`
- `EmployerProfileId`
- `JobCategoryId`
- `Title`
- `Description`
- `EmploymentType`
- `SalaryModel`
- `SalaryMin`
- `SalaryMax`
- `OpeningsCount`
- `MinExperienceMonths`
- `Status`
- `ApplicationDeadline`
- `PublishedAt`
- `Metadata`

Important behavior:

- Requires title and description.
- Requires openings count greater than zero.
- Validates salary min/max.
- Starts as draft.
- Can publish, pause, close, mark filled, cancel, and set deadline.

Code example:

```csharp
public static Job Create(
    Guid employerProfileId,
    string title,
    string description,
    EmploymentType employmentType,
    SalaryModel salaryModel,
    decimal? salaryMin = null,
    decimal? salaryMax = null,
    int openingsCount = 1,
    int minExperienceMonths = 0,
    Guid? jobCategoryId = null)
{
    if (string.IsNullOrWhiteSpace(title))
    {
        throw new InvalidOperationException("Title is required.");
    }

    if (salaryMin.HasValue && salaryMax.HasValue && salaryMin > salaryMax)
    {
        throw new InvalidOperationException(
            "Minimum salary cannot exceed maximum salary.");
    }

    return new Job
    {
        JobId = Guid.NewGuid(),
        EmployerProfileId = employerProfileId,
        JobCategoryId = jobCategoryId,
        Title = title,
        Description = description,
        EmploymentType = employmentType,
        SalaryModel = salaryModel,
        SalaryMin = salaryMin,
        SalaryMax = salaryMax,
        OpeningsCount = openingsCount,
        MinExperienceMonths = minExperienceMonths,
        Status = JobStatus.Draft,
        CreatedAt = DateTimeOffset.UtcNow,
        UpdatedAt = DateTimeOffset.UtcNow
    };
}
```

### Other Domain Areas

Accommodation:

- `AccommodationProvider`
- `Property`
- `Room`
- `RoomType`
- `RoomAvailability`
- `AccommodationBooking`
- `Facility`
- `PropertyFacility`
- `PropertyImage`

Food:

- `FoodProvider`
- `FoodItem`
- `FoodPlan`
- `FoodPlanItem`
- `FoodSubscription`
- `DeliveryArea`

Messaging:

- `Conversation`
- `ConversationParticipant`
- `Message`
- `MessageAttachment`

Billing:

- `BillingSubscription`
- `Invoice`
- `Payment`
- `Transaction`

Trust, safety, and analytics:

- `Review`
- `Complaint`
- `Report`
- `AuditLog`
- `AnalyticsEvent`
- `UserBehaviorEvent`
- `SearchHistory`
- `RecommendationHistory`
- `UserPreference`

## 11. Database Design

Migration path:

`D:\Practical Project\database_migrations`

Execution order:

1. `00_extensions_enums.sql`
2. `01_identity_access.sql`
3. `02_location.sql`
4. `03_worker_profile.sql`
5. `04_job_marketplace.sql`
6. `05_matching_ai.sql`
7. `06_accommodation.sql`
8. `07_food_service.sql`
9. `08_communication.sql`
10. `09_payments.sql`
11. `10_reviews_admin_analytics.sql`
12. `98_table_comments.sql`
13. `99_indexes.sql`

PostgreSQL extensions:

```sql
CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE EXTENSION IF NOT EXISTS citext;
CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS vector;
```

PostgreSQL enum types:

- `account_status`
- `verification_channel`
- `verification_status`
- `session_status`
- `employment_type`
- `salary_model`
- `job_status`
- `application_status`
- `contract_status`
- `booking_status`
- `payment_status`
- `subscription_status`
- `notification_status`
- `conversation_type`
- `review_status`
- `complaint_status`

### Database Modules and Tables

Identity and access:

- `users`
- `roles`
- `permissions`
- `user_roles`
- `role_permissions`
- `user_profiles`
- `verifications`
- `user_sessions`
- `user_preferences`
- `audit_logs`

Location:

- `countries`
- `states`
- `cities`
- `areas`
- `locations`

Worker profile:

- `worker_profiles`
- `skills`
- `worker_skills`
- `worker_availability`
- `worker_experience`
- `worker_education`
- `worker_documents`

Job marketplace:

- `employer_profiles`
- `job_categories`
- `jobs`
- `job_skills`
- `job_locations`
- `job_schedules`
- `job_applications`
- `shortlists`
- `hiring_status_history`
- `contracts`

Matching and AI:

- `matching_scores`
- `recommendation_history`
- `search_history`
- `user_behavior_events`
- `entity_embeddings`

Accommodation:

- `accommodation_providers`
- `room_types`
- `properties`
- `rooms`
- `facilities`
- `property_facilities`
- `room_availability`
- `accommodation_bookings`
- `property_images`

Food service:

- `food_providers`
- `food_items`
- `food_plans`
- `food_plan_items`
- `food_subscriptions`
- `delivery_areas`

Communication:

- `conversations`
- `conversation_participants`
- `messages`
- `message_attachments`
- `notifications`

Payments:

- `billing_subscriptions`
- `invoices`
- `payments`
- `transactions`

Reviews, admin, analytics:

- `reviews`
- `reports`
- `complaints`
- `analytics_events`

### Relationship Summary

- `users` is the base identity table.
- `users` can have many `roles` through `user_roles`.
- `roles` can have many `permissions` through `role_permissions`.
- `users` have profile, session, verification, preference, and audit records.
- Worker, employer, accommodation provider, and food provider records extend `users`.
- Employers post jobs.
- Jobs can have skills, locations, schedules, applications, shortlists, history, and contracts.
- Worker profiles can have skills, availability, experience, education, documents, applications, contracts, and matching scores.
- Locations are reusable across users, jobs, properties, and providers.
- Accommodation providers manage properties and rooms.
- Food providers manage items, plans, subscriptions, and delivery areas.
- Conversations contain participants, messages, and attachments.
- Payments can reference different payable entity types.
- Reviews, complaints, reports, analytics, and recommendations use generic entity references for platform-wide reuse.

### Indexing Strategy

The schema includes indexes for:

- User status and last active date.
- Role and permission bridge lookup.
- Verification/session user status lookup.
- Location hierarchy and geospatial search.
- Worker location, rating, skill, availability, and document status.
- Job employer/status, category/status, open jobs, skills, locations, schedules, applications, and contracts.
- Matching score by job and worker.
- Recommendations, searches, and behavior events.
- Accommodation provider, property, room availability, and bookings.
- Food provider, item availability, plans, subscriptions, and delivery areas.
- Conversation and message retrieval.
- Notification status and unread notifications.
- Billing subscriptions, invoices, payments, transactions.
- Reviews, reports, complaints, analytics events.
- JSONB fields using GIN indexes.
- Vector embeddings using `ivfflat`.

Example indexes:

```sql
CREATE INDEX idx_jobs_open_published
ON jobs(published_at DESC)
WHERE status = 'open';

CREATE INDEX idx_job_locations_geo
ON job_locations USING GIST (geo_point);

CREATE INDEX idx_embeddings_vector
ON entity_embeddings USING ivfflat (embedding vector_cosine_ops)
WITH (lists = 100);

CREATE INDEX idx_notifications_unread
ON notifications(user_id, created_at DESC)
WHERE read_at IS NULL;
```

## 12. Frontend Console

Frontend path:

`PTimeJobs/src/PTimeJobs.Web`

Purpose:

- Provides an operations dashboard for marketplace administrators.
- Shows module navigation for overview, jobs, workers, accommodation, food, messages, payments, and security.
- Displays dashboard metrics, operational queues, matching examples, recent activity, and API connection status.

Main frontend files:

- `src/App.tsx`
- `src/api.ts`
- `src/styles.css`
- `package.json`
- `vite.config.ts`

API client:

```ts
export type HealthResponse = {
  status: string;
  message: string;
  data?: {
    service: string;
    databaseConnected: boolean;
    checkedAt: string;
  };
};

const apiBaseUrl =
  import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:7101';

export async function getHealth(signal?: AbortSignal): Promise<HealthResponse> {
  const response = await fetch(`${apiBaseUrl}/api/v1/health`, { signal });

  if (!response.ok) {
    throw new Error(`API returned ${response.status}`);
  }

  return response.json();
}
```

Dashboard modules currently shown in UI:

- Job marketplace
- Worker management
- Accommodation
- Food services
- Payments
- Notifications

Frontend scripts:

```json
{
  "dev": "vite --host 127.0.0.1",
  "build": "tsc -b && vite build",
  "preview": "vite preview --host 127.0.0.1"
}
```

## 13. Configuration

Default database connection string from project README:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ptimejobs;Username=postgres;Password=postgres"
  }
}
```

Update this in:

`PTimeJobs/src/PTimeJobs.Api/appsettings.Development.json`

Frontend API base URL:

```text
VITE_API_BASE_URL=https://localhost:7101
```

When not set, the frontend defaults to:

```text
https://localhost:7101
```

## 14. How to Run

Backend:

```bash
cd PTimeJobs/src/PTimeJobs.Api
dotnet run
```

Swagger in development:

```text
https://localhost:<api-port>/swagger
```

Health endpoint:

```http
GET /api/v1/health
```

Frontend:

```bash
cd PTimeJobs/src/PTimeJobs.Web
npm install
npm run dev
```

Default frontend URL:

```text
http://127.0.0.1:5173
```

Frontend production build:

```bash
npm run build
```

Tests:

```bash
cd PTimeJobs
dotnet test
```

## 15. Current Tests

File:

`PTimeJobs/tests/PTimeJobs.Tests/ApiResponseTests.cs`

Covered behavior:

- `ApiResponse<T>.Success` creates a success response with standard message, data, and no errors.
- `ApiResponse<T>.Failure` creates an error response with message and no data.

Code example:

```csharp
[Fact]
public void Success_ShouldCreateStandardSuccessResponse()
{
    var response = ApiResponse<string>.Success("ok");

    Assert.Equal("success", response.Status);
    Assert.Equal("Operation completed successfully.", response.Message);
    Assert.Equal("ok", response.Data);
    Assert.Null(response.Errors);
}
```

## 16. Implemented vs Planned Features

Implemented in code now:

- Clean Architecture project structure.
- ASP.NET Core API startup.
- Swagger setup.
- JWT bearer authentication registration.
- CORS for Vite frontend.
- Global exception middleware.
- Standard `ApiResponse<T>` model.
- Health endpoint with database connectivity check.
- Admin-only user summary endpoint.
- EF Core DbContext covering platform modules.
- PostgreSQL enum mapping.
- Generic repository and unit of work.
- User query service projection.
- React/Vite operations dashboard.
- Frontend health API integration.
- Unit tests for API response model.

Designed in domain and database schema:

- Identity and access control.
- Worker profiles and worker lifecycle.
- Employer profiles and job marketplace.
- Job applications, shortlists, status history, and contracts.
- AI matching and recommendation tracking.
- Accommodation marketplace and room booking.
- Food services and subscriptions.
- Messaging and notifications.
- Payments, invoices, subscriptions, and transactions.
- Reviews, complaints, reports, audit logs, analytics, search, and user behavior tracking.

Not yet fully exposed as API workflows:

- Authentication endpoints.
- User registration and login.
- Worker profile CRUD.
- Employer CRUD.
- Job posting and search endpoints.
- Application and hiring workflow endpoints.
- Accommodation CRUD and booking endpoints.
- Food provider, plan, and subscription endpoints.
- Messaging endpoints.
- Notification delivery workers.
- Payment gateway integration.
- Review, complaint, and reporting workflows.
- AI matching service implementation.
- Admin analytics endpoints.

## 17. Recommended Next Development Steps

1. Add authentication workflows: register, login, refresh token, logout, password reset.
2. Add request/response DTOs and validators for user, worker, employer, and job modules.
3. Expose job marketplace APIs: create job, publish job, search jobs, apply, shortlist, hire.
4. Add EF Core migrations or align SQL migration execution with the current DbContext mappings.
5. Add role and permission authorization policies.
6. Expand tests around domain rules and API controllers.
7. Add frontend pages for the real workflows currently represented as dashboard queues.
8. Add background jobs for notifications, AI matching, analytics ingestion, and billing events.
9. Add payment provider integration behind application-layer service interfaces.
10. Add operational logging, structured audit events, and security-sensitive activity tracking.

## 18. Summary

PTimeJobs is designed as a marketplace platform for part-time jobs plus surrounding living needs such as accommodation and food. The repository already contains a strong backend architecture foundation, broad domain model, PostgreSQL database design, EF Core mappings, a starter API, a health-checked frontend console, and unit-test scaffolding.

The next major work is to convert the existing domain and database design into complete application workflows by adding DTOs, validators, use-case services, endpoints, authorization policies, frontend screens, and tests module by module.
