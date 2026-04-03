# ADR 0006: CQRS via MediatR and Data repositories

| Field | Value |
|-------|--------|
| **Status** | Accepted |
| **Date** | 2026-04-03 |

## Context

We need a consistent way to organize **reads and writes** without fat controllers and without leaking EF details into the HTTP layer. Handlers should be testable and share a common pipeline (validation, logging, etc.) as the app grows.

## Decision

1. **Application API:** ASP.NET **controllers** are thin: they accept HTTP input, map to **commands/queries**, and call **`IMediator.Send`**.
2. **Messages:** **Commands** and **queries** live under `MotoklubBezbednost.Business/Cqrs/` (grouped by feature: Members, Trainings, …).
3. **Handlers:** One handler per message (or a documented exception), implementing `IRequestHandler<,>`.
4. **Persistence:** **`MotoklubBezbednost.Data`** exposes **scoped repositories** (e.g. `IMemberRepository`) registered in `Program.cs`. Handlers depend on these abstractions rather than `DbContext` directly where possible.
5. **Optional evolution:** A **unit-of-work** façade could be introduced later without changing the MediatR shape; until then, repositories encapsulate queries and writes against the shared context.

## Consequences

- **Positive:** Clear feature slices; controllers stay small; handlers are the natural place for rules and orchestration.
- **Negative:** More types and indirection than a minimal three-file CRUD demo; boilerplate for new features.

## Alternatives considered

- **Direct `DbContext` in controllers:** Fast for prototypes; rejected for boundary clarity and testability.
- **Vertical slice architecture (single file per feature):** Possible future refactor; current structure uses explicit `Cqrs/` folders per aggregate.
