# ADR 0003: Monorepo composition and backend layering

| Field | Value |
|-------|--------|
| **Status** | Accepted |
| **Date** | 2026-04-03 |

## Context

The system is delivered as **one git repository** containing the API host, domain logic, persistence, tests, SPA, scripts, and documentation. We need clear **physical boundaries** so dependencies flow in one direction and the SPA stays a separate build artifact consumed by the API in packaged builds.

## Decision

### Repository layout (monorepo)

| Area | Path | Role |
|------|------|------|
| API host | `backend/MotoklubBezbednost.API/` | HTTP entrypoint, DI composition, controllers, auth/CORS, static files in production |
| Domain / application | `backend/MotoklubBezbednost.Business/` | MediatR commands/queries/handlers, DTOs, mappings |
| Persistence | `backend/MotoklubBezbednost.Data/` | `DbContext`, EF models, migrations, repositories |
| Tests | `backend/MotoklubBezbednost.API.Tests/` | Automated tests (expand over time) |
| SPA | `frontend/` | React source; build output is published into the API’s web root for local bundles |
| Automation | `scripts/` | Packaging, dev startup, maintenance scripts |
| Docs | `docs/adr/`, `docs/plans/` | ADRs and planning notes |

### Dependency rules

1. **`MotoklubBezbednost.API`** references **Business** and **Data** (for `DbContext` registration and infrastructure wiring).
2. **`MotoklubBezbednost.Business`** references **Data** for repositories and entities used inside handlers.
3. **`MotoklubBezbednost.Data`** does **not** reference API or Business.
4. Controllers **do not** embed business rules or direct EF queries for feature work—use **MediatR** and DTOs.

### Frontend boundary

The SPA is a **separate project** built with npm; it talks to the API over HTTP (development proxy, production same-origin when hosted by the API). Shared contracts are implied by TypeScript types and API DTOs, not by a shared .NET project.

## Consequences

- **Positive:** Clear layering; Data can be tested and evolved with a stable surface to Business.
- **Negative:** DTOs and TS types can drift—changes need coordinated API + frontend updates; consider OpenAPI later if duplication hurts.

## Alternatives considered

- **Single ASP.NET project** (controllers + EF in one assembly): Rejected—harder to test and to keep persistence out of HTTP layer.
- **Separate frontend repo:** Rejected for this product—local packaging and version alignment are simpler in one repo.
