# Database Migration Convention

This folder is the official migration/query location for future backend features.

The existing database design should not be redesigned unnecessarily. If a feature needs a database change, create a new versioned SQL file under the relevant module folder.

## Folder Structure

- `User/` - identity, roles, profiles, verification, sessions, preferences
- `Job/` - jobs, applications, hiring workflow, contracts, worker-job matching data
- `Accommodation/` - properties, rooms, facilities, availability, bookings
- `Food/` - food providers, food items, meal plans, subscriptions, delivery areas
- `Payment/` - payments, invoices, transactions, billing subscriptions
- `Notification/` - notifications, message delivery, email, SMS, push-related changes
- `AI/` - matching, recommendation logs, embeddings, behavior/search signals
- `Common/` - shared lookup tables, locations, audit, cross-module utilities

## Naming Rule

Use version-based migration names:

```text
Database_Migration/
  Job/
    001_Create_Jobs_Table.sql
    002_Add_Job_Status.sql
    003_Add_Job_Index.sql
```

Prefer one logical database change per file. Do not put all module changes into one large script.

## Required Migration Contents

Each migration file should include:

- Purpose comment at the top
- Forward SQL change
- Constraints, indexes, and foreign keys where applicable
- Safe defaults for existing data when adding non-null columns
- Rollback notes as comments if automatic rollback is risky

## Database Change Rules

- Do not directly modify production tables outside migration files.
- Do not redesign existing tables unless the business requirement clearly demands it.
- Prefer additive changes for backward compatibility.
- Use indexes only for real query patterns.
- Keep module ownership clean so future microservice extraction remains possible.
