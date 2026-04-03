# Motoklub Bezbednost API

ASP.NET Core Web API for managing motorcycle club members, motorcycles, equipment, and training records.

## Prerequisites

- .NET 8.0 SDK

## Database

SQLite files live under `data/` next to the API content root (see `Motoklub:SqliteFileName` in `appsettings*.json`). Three profiles are provided:

| ASP.NET environment | SQLite file (default) |
|---------------------|------------------------|
| `Test` | `motoklub_test.db` |
| `LocalStaging` | `motoklub_staging.db` |
| `LocalProd` | `motoklub.db` |

Development uses `appsettings.Development.json` (staging DB + `AutoMigrate`).

## Auth

JWT is signed locally (`Jwt` section in `appsettings.json`). Credentials are in `LocalAuth` (default `admin` / `change-me` — change for real use).

Login: `POST /api/auth/login` with `{ "username", "password" }`.

## Migrations

```bash
cd backend
dotnet ef database update --project MotoklubBezbednost.Data --startup-project MotoklubBezbednost.API
```

Or enable `Motoklub:AutoMigrate` so the API applies migrations on startup.

## Running

```bash
cd backend/MotoklubBezbednost.API
dotnet run
```

The repo **`scripts/Start-Motoklub.ps1`** always uses **`LocalProd`** and **`motoklub.db`**. Visual Studio / `launchSettings.json` profiles **SQLite-Test**, **SQLite-Staging**, **SQLite-Prod** pick other environments when you run from the IDE.

## API endpoints

- `/api/auth/login` — obtain JWT
- `/api/members`, `/api/motorcycles`, `/api/trainings`, `/api/equipment` — require `Authorization: Bearer …`
