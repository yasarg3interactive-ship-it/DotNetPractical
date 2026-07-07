# Feature Intake Template

Use this before implementing a new feature.

## Requirement Questions

1. What business problem should this feature solve?
2. Which users or roles will use it?
3. What is the expected user flow from start to finish?
4. What business rules must be enforced?
5. What edge cases should be handled?
6. What security rules are required?
7. What performance expectations apply?
8. Does this feature need notifications, payments, AI, search, or analytics?
9. Is a database migration required?
10. What future expansion should this design allow?

## Architecture Checklist

### Domain Layer

- Entities
- Value objects
- Enums
- Domain rules
- Domain events

### Application Layer

- Interfaces
- DTOs
- Validators
- Use cases
- Services

### Infrastructure Layer

- EF Core mappings
- Repository implementations
- Database migrations
- External integrations
- Cache/background jobs

### API Layer

- Controllers
- Request models
- Response models
- Authorization policies
- Error handling
- Swagger/OpenAPI notes

## Database Checklist

- Use existing schema if possible.
- Create only migration/update files when schema changes are required.
- Place migration files under the correct `Database_Migration/` module folder.
- Use version-based file names such as `001_AddJobPriority.sql`.
- Keep each file focused on one logical change.

## Delivery Checklist

- Architecture explanation
- Database impact
- Migration files required
- Backend implementation summary
- API documentation
- Testing approach
- Future improvements
