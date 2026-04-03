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

Output: `frontend/dist`. For portable packaging, use the repo script `scripts/Package-Local.ps1`, which copies this build into the published API `wwwroot`.
