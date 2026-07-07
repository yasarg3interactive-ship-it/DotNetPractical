# PTimeJobs Backend

ASP.NET Core 8 backend starter for the Part-Time Work & Living Marketplace Platform.

## Architecture

- `PTimeJobs.Domain` - entities, enums, domain rules
- `PTimeJobs.Application` - DTOs, service/repository contracts, use-case abstractions
- `PTimeJobs.Infrastructure` - EF Core DbContext, repository implementations, external services
- `PTimeJobs.Api` - controllers, middleware, authentication, response formatting
- `PTimeJobs.Web` - React/Vite frontend console
- `PTimeJobs.Tests` - unit-test project

## Database

The API is configured for PostgreSQL.

Default connection string:

```json
"DefaultConnection": "Host=localhost;Port=5432;Database=ptimejobs;Username=postgres;Password=postgres"
```

Update `src/PTimeJobs.Api/appsettings.Development.json` for your local database.

Existing SQL migration scripts are in the root-level `database_migrations/` folder. Future update scripts should go under root-level `Database_Migration/` by module.

## Starter Endpoint

```http
GET /api/v1/health
```

Returns the standard API response format and checks database connectivity.

## Notes

- This project targets `net8.0`.
- Database schema is not recreated here. EF Core mappings currently connect to the existing `users`, `roles`, and `user_roles` tables as the starter identity foundation.

## Frontend

The frontend is in `src/PTimeJobs.Web`.

```bash
cd src/PTimeJobs.Web
npm install
npm run dev
```

Default URL:

```text
http://127.0.0.1:5173
```

Production build:

```bash
npm run build
```

Set `VITE_API_BASE_URL` if the API runs on a different host or port.
