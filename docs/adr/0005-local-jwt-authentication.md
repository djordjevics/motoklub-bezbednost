# ADR 0005: Local JWT authentication

| Field | Value |
|-------|--------|
| **Status** | Accepted |
| **Date** | 2026-04-03 |

## Context

API endpoints for club data must be **authenticated** in deployed scenarios, but the product does **not** depend on cloud identity providers (Azure AD, Auth0, etc.). Operators need a simple model: configure secrets locally and issue tokens from this API only.

## Decision

1. **Scheme:** **JWT Bearer** authentication using **symmetric** signing (`Jwt` section in `appsettings`: issuer, audience, signing key).
2. **Login:** Credentials validated against **local configuration** (`LocalAuth` or equivalent)—not a user store in DB unless a future ADR adds one.
3. **Clients:** The React app obtains a token (e.g. `POST /api/auth/login`) and sends **`Authorization: Bearer …`** on subsequent API calls.
4. **CORS:** Configured to allow the SPA origin(s) in development and controlled origins in deployment.

**Operational rule:** Change default passwords and signing keys before any real deployment; treat `appsettings` secrets as environment-specific.

## Consequences

- **Positive:** No external auth dependency; works offline; easy to reason about for a small install base.
- **Negative:** Key rotation and account lifecycle are manual unless we add an ADR for multi-user DB-backed identity.

## Alternatives considered

- **Cookies + same-site sessions only:** Viable for same-origin SPA; JWT matches current implementation and simple bearer usage from axios.
- **Cloud OIDC:** Deferred—out of scope for local-first packaging.
