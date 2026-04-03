# Motoklub Bezbednost Frontend

React + TypeScript + Vite. The UI talks to the API only over HTTP (no direct database access).

## Prerequisites

- Node.js 24.14 and npm

## Setup

```bash
npm install
```

Optional `.env` (defaults work for local dev with the Vite proxy):

```
VITE_API_BASE_URL=/api
```

For a production build served from the same host as the API (see `scripts/Package-Local.ps1`), use `VITE_API_BASE_URL=/api`.

## Running

Terminal 1 — API on port 5000. Terminal 2:

```bash
npm run dev
```

Open `http://localhost:3000`. API requests go to `/api` and are proxied to `http://localhost:5000`.

## Build

```bash
npm run build
```

`prebuild` runs **Aikido Safe Chain** checks (see below). Output: `frontend/dist`. For portable packaging, use `scripts/Package-Local.ps1`, which copies this build into the published API `wwwroot`.

### Aikido Safe Chain (npm hardening)

Install [Safe Chain](https://github.com/AikidoSec/safe-chain) on your machine so `npm` / `npx` are wrapped (malware + minimum package age). After install, restart the terminal and run `npm safe-chain-verify`.

Before **`npm run build`**, the repo runs `scripts/safe-chain-preflight.mjs`, which:

1. Ensures `safe-chain` is on `PATH`
2. Runs `npm safe-chain-verify`
3. Compares the CLI version to **`npm view @aikidosec/safe-chain version`** (same major.minor.patch)

There is **no** opt-out: every `npm run build` (including `Package-Local.ps1`) requires Safe Chain on `PATH`, a working `npm safe-chain-verify`, and a matching semver vs npm latest when the registry is reachable. **CI must install Safe Chain** (see the [Safe Chain CI docs](https://github.com/AikidoSec/safe-chain#usage-in-cicd)) before `npm ci` / `npm run build`.

---

## Maintenance guidelines

These notes mirror **`.cursor/rules/frontend-maintenance.mdc`** so humans and automation share one standard.

### Reporting issues (product / QA)

Include when possible:

- **Route or URL** and **steps to reproduce**
- **Expected vs actual** behavior
- **Browser** (and version if known)
- Whether **DevTools → Network** shows failed requests, **401**, **403**, **4xx**, or **5xx**

### Implementing changes

1. **HTTP:** Use **`apiClient`** only (`src/services/apiClient.ts`). Add methods on feature services (`memberService`, etc.), not scattered `axios`/`fetch`.
2. **Types:** Keep **`src/types/*`** aligned with API JSON shapes when endpoints or DTOs change.
3. **Server state:** Use **TanStack Query** (`useQuery` / `useMutation`). Use stable **`queryKey`** hierarchies (e.g. `['members']`, `['members', id]`). Handle **loading**, **error**, and **empty** states. Invalidate queries after mutations that affect lists or detail.
4. **Forms:** Prefer **react-hook-form** + **yup** + **MUI** for new or expanded forms.
5. **UI:** Stay on **MUI** and existing **Layout** / **theme** patterns unless the team decides otherwise (document in an ADR).
6. **Routes:** New authenticated pages go under the **`Layout`** + **`ProtectedRoute`** branch in `App.tsx`; add navigation when users must find the page.
7. **Secrets:** Never put secrets in the frontend. Only **`VITE_*`** environment variables are exposed to the client.
8. **Before merge:** `npm run lint` and `npm run build` must succeed (build always runs Safe Chain preflight; see **Aikido Safe Chain** above).

### Backend coordination

API changes (routes, bodies, response fields) should land with matching updates to **`services/`** and **`types/`** (same PR or a clearly linked follow-up).
