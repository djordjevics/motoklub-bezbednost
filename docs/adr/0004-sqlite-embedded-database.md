# ADR 0004: SQLite as embedded database

| Field | Value |
|-------|--------|
| **Status** | Accepted |
| **Date** | 2026-04-03 |

## Context

The application targets **local or portable deployment** (single folder, minimal prerequisites). The database must be **easy to back up**, ship with the app, and run without installing a database server.

## Decision

Use **SQLite** as the **only** production database for this product line, accessed through **EF Core** with the **Microsoft.EntityFrameworkCore.Sqlite** provider.

- Database files live under the API **content root** `data/` (exact filename controlled by configuration, e.g. `Motoklub:SqliteFileName` and environment-specific `appsettings`).
- Schema changes are delivered via **EF Core migrations** in `MotoklubBezbednost.Data/Migrations/`.
- **Auto-migrate on startup** may be enabled in some environments; packaged installs still rely on migrations being applied on upgrade (see root README).

## Consequences

- **Positive:** No separate DB service; trivial file backup (`motoklub.db`); fits offline clubs and single-machine installs.
- **Negative:** SQLite concurrency and advanced features differ from server RDBMSs; scaling to many simultaneous writers is not a goal of this ADR.

## Alternatives considered

- **PostgreSQL / SQL Server:** Better for multi-tenant hosted scenarios; heavier ops for local-only users.
- **LiteDB / JSON files:** Rejected—team standard is relational model + EF migrations for evolving schema.
