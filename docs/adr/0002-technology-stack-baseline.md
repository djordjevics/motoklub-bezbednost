# ADR 0002: Technology stack baseline

| Field | Value |
|-------|--------|
| **Status** | Accepted |
| **Date** | 2026-04-03 |

## Context

The product is a **local-first** club administration tool: a browser UI talking to an on-machine (or LAN) API. We need a stack that is easy to deploy without external services, matches team skills, and stays maintainable in a single repository.

## Decision

Adopt the following **baseline stack** (versions may advance in lockfiles; supersede this ADR if the baseline changes materially).

### Backend

- **Runtime / language:** C# on **.NET 8** (`net8.0`), nullable reference types enabled.
- **Web host:** **ASP.NET Core** minimal hosting + controllers.
- **Persistence:** **EF Core 8** with **SQLite** provider (see [ADR 0004](0004-sqlite-embedded-database.md)).
- **API style:** REST-style JSON under `/api`, **Swagger** in development.
- **In-process patterns:** **MediatR** for CQRS-style handlers; **FluentValidation.AspNetCore** available for request validation.
- **Auth:** **JWT Bearer** with locally configured signing key (see [ADR 0005](0005-local-jwt-authentication.md)).

### Frontend

- **Runtime:** **Node.js** ≥ 24 (see `frontend/package.json` `engines`).
- **UI library:** **React 18** with **TypeScript**.
- **Build / dev server:** **Vite** (~6.x).
- **Routing:** **react-router-dom**.
- **HTTP / server state:** **axios**, **TanStack Query**.
- **Forms / validation:** **react-hook-form**, **yup**, **@hookform/resolvers**.
- **Components:** **MUI** (`@mui/material`, Emotion).

### Tooling and delivery

- **Scripts:** **PowerShell 7+** (`pwsh`) for local run and packaging; optional **GNU Make** as a thin wrapper.
- **Package output:** Published API + built SPA under `wwwroot` + starter scripts (see [ADR 0007](0007-local-packaging-and-distribution.md)).

## Consequences

- **Positive:** One coherent “.NET + React” story; offline-friendly; small operational surface.
- **Negative:** Stack upgrades (major .NET / Node / Vite) should be deliberate and may warrant a new ADR or supersession note.

## Alternatives considered

- **Blazor instead of React:** Rejected for this codebase—SPA + REST matches current structure and ecosystem choices.
- **PostgreSQL / SQL Server for v1 local bundle:** Rejected for minimal install footprint; SQLite fits single-file deployment (see ADR 0004).
