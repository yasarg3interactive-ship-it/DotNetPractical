# Backend Architecture Guide

Project: Part-Time Work & Living Marketplace Platform

Technology stack:

- ASP.NET Core Web API
- C#
- Entity Framework Core
- Clean Architecture
- Domain Driven Design principles
- Repository Pattern
- Dependency Injection

## Development Workflow

For every new feature, work in this order:

1. Requirement analysis
2. System design
3. Implementation plan
4. Code implementation
5. API documentation
6. Testing approach

Before implementation, clarify:

- What is the business purpose?
- Who will use this feature?
- What is the expected user flow?
- Business problem and success criteria
- Business rules and edge cases
- Security and authorization requirements
- Performance and scale expectations
- Future expansion requirements
- Whether a database migration is required

Do not generate feature code until these questions are answered or the requirement is already explicit enough to proceed safely.

## Clean Architecture Layers

### Domain Layer

Contains enterprise business rules:

- Entities
- Value objects
- Domain events
- Domain rules
- Domain exceptions

The domain layer must not depend on EF Core, controllers, external APIs, or infrastructure services.

### Application Layer

Contains use cases and orchestration:

- DTOs
- Commands and queries
- Validators
- Service interfaces
- Repository interfaces
- Use-case handlers

Business logic belongs here or in the domain layer, not in controllers.

### Infrastructure Layer

Contains technical implementations:

- EF Core DbContext
- Repository implementations
- External providers
- Email, SMS, push integrations
- Payment gateway integrations
- Redis/cache implementations
- Background job implementations

### API Layer

Contains delivery concerns:

- Controllers or minimal API endpoints
- Request/response contracts
- Authentication and authorization setup
- Middleware
- Error response formatting
- Swagger/OpenAPI documentation

Controllers should stay thin and delegate work to application services or handlers.

## API Rules

Every API should include:

- Request DTO
- Response DTO
- Input validation
- Service/use-case layer call
- Repository abstraction where persistence is needed
- Proper HTTP status codes
- Consistent error response format
- Logging at useful boundaries
- Async methods for I/O work

Standard success response:

```json
{
  "status": "success",
  "message": "Operation completed successfully.",
  "data": {}
}
```

Standard error response:

```json
{
  "status": "error",
  "message": "Validation failed.",
  "errors": {}
}
```

Never expose EF Core entities directly from API responses.

## Security Baseline

Implement:

- JWT authentication
- Refresh tokens
- Role-based authorization
- Input validation
- Secure password hashing
- OWASP-conscious API design
- Safe file upload handling
- Rate limiting for authentication and sensitive endpoints
- Audit logs for admin and security-sensitive actions
- Permission-based access for admin and sensitive marketplace workflows
- Secure handling for documents, resumes, images, and payment references

## Performance Baseline

Consider:

- Pagination for list APIs
- Projection queries instead of loading full graphs
- Database indexes for common filters
- Redis caching for hot read models
- Background jobs for slow workflows
- Query optimization and avoiding N+1 calls
- Read models/search indexes for marketplace discovery at scale
- Efficient joins and filtered indexes for marketplace search
- Message queues for notifications, matching, billing, and analytics workflows

## Feature Design Checklist

Before adding a feature, confirm:

- Does the existing architecture support this?
- Is a database change required?
- Which module owns the feature?
- Which roles can use the feature?
- What are the edge cases?
- What should be logged or audited?
- Does this need notifications?
- Does this need payment or billing integration?
- Does this need AI/search/matching data?
- Will the design remain microservice-ready?

## Migration Rule

Future database changes must go under `Database_Migration/` by module. Do not place new migration files in one combined SQL script.

## Code Generation Rules

For every feature implementation, create only the layers that are needed:

- Entity or value object in the Domain layer when the business model changes.
- Request and response DTOs in the Application/API boundary.
- Service interfaces and implementations for business workflows.
- Repository interfaces and implementations for persistence.
- Controller endpoints that only receive requests, call the application layer, and return responses.
- Validators for request rules and business-safe input.

Avoid:

- Fat controllers
- Business logic in controllers
- Direct database queries from the API layer
- Hardcoded values
- Temporary shortcuts
- Duplicate logic
- Unnecessary patterns that do not match the existing project

## Testing Standard

Important business logic should be unit-test friendly and mockable.

Cover:

- Success cases
- Validation failures
- Authorization failures
- Edge cases
- Repository/service exception paths where meaningful

## Feature Documentation Output

For every completed feature, provide:

- Feature explanation
- Architecture changes
- Database changes
- API documentation
- Code changes summary
- Testing notes
- Future improvement possibilities
