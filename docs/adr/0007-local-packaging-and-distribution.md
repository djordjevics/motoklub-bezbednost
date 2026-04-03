# ADR 0007: Local packaging and distribution

| Field | Value |
|-------|--------|
| **Status** | Accepted |
| **Date** | 2026-04-03 |

## Context

Users may run the app on a **single Windows PC** (or similar) without cloning the repository. Builds must produce a **portable folder**: API binaries, embedded SPA, starter script, and a predictable SQLite path so upgrades do not destroy data.

## Decision

1. **Build pipeline (developer machine):** PowerShell scripts under `scripts/` (e.g. `Package-Local.ps1`) build the frontend (`npm run build`), publish the API, copy static assets to `wwwroot`, and emit a tree under **`dist/motoklub-local`** (or update bundles under `dist/motoklub-update`).
2. **Runtime:** **`Start-Motoklub.ps1`** sets environment so the API uses **`LocalProd`-style** settings, a **single SQLite file** under `./data/motoklub.db`, and applies migrations as configured.
3. **Updates:** Update packages **omit** `data/`; operators copy files over the install and **preserve** the existing database file.
4. **Optional:** **Self-contained** publish (`win-x64` etc.) to avoid a separate .NET runtime install on the target machine—trade-off is larger output size.

## Consequences

- **Positive:** Repeatable installs; clear separation of **code** vs **data**; scriptable upgrades.
- **Negative:** Packaging logic must stay in sync with README and CI (if added later); test packaging after major template changes.

## Alternatives considered

- **Docker-only distribution:** Useful for some ops teams; not the primary path for the documented local Windows workflow.
- **ClickOnce / MSI:** Deferred—folder + PowerShell matches current repo automation.
