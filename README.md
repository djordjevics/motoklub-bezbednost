# motoklub-bezbednost

Local-only stack: **ASP.NET Core** backend (SQLite + EF Core migrations), **React/Vite** frontend. Authentication is **local JWT** (no cloud providers).

## Quick start (development)

1. **Backend** — from `backend/MotoklubBezbednost.API`: `dotnet run` (uses `data/motoklub_staging.db` with auto-migrate in Development). Change `Jwt`/`LocalAuth` in `appsettings.json` before real use.
2. **Frontend** — from `frontend` (Node.js **24.14**): `npm install` then `npm run dev` → `http://localhost:3000` (proxies `/api` to `http://localhost:5000`).

Sign in with the credentials from `LocalAuth` (default `admin` / `change-me`).

## SQLite database file

**`Start-Motoklub.ps1`** (packaged or `-Dev`) sets `Motoklub__SqliteFileName` to **`motoklub.db`** under the API `data/` folder and **`ASPNETCORE_ENVIRONMENT=LocalProd`**.

Optional Visual Studio launch profiles (**SQLite-Test**, **SQLite-Staging**, **SQLite-Prod**) still map to other `appsettings.*` files and DB names if you run the API from the IDE instead of the script.

## Portable package (another PC)

**Build machine:** .NET 8 SDK, Node.js **24.14** and npm, PowerShell 7+ (`pwsh`). Optional: GNU Make (`make package`) or use the scripts below directly.

From repo root, build a full folder under `dist/motoklub-local` (API + `wwwroot` SPA + starter script):

```powershell
pwsh ./scripts/Package-Local.ps1
```

Or:

```text
make package
```

**Target machine:** Install [.NET 8 runtime](https://dotnet.microsoft.com/download/dotnet/8.0), or build with `make package-self-contained` / `pwsh ./scripts/Package-Local.ps1 -SelfContained -Runtime win-x64` so the output includes the runtime (larger, no separate .NET install).

On first run, SQLite is created under `data/` and EF migrations apply automatically (`Motoklub__AutoMigrate` is set by the start script). No separate migration tool is required.

Then run the published folder (backup + migrate + API + browser):

```powershell
pwsh -File ./dist/motoklub-local/Start-Motoklub.ps1
```

Or open `dist/motoklub-local` in PowerShell and run:

```powershell
.\Start-Motoklub.ps1
```

`Start-Motoklub.ps1` always uses **`.\data\motoklub.db`** with **`LocalProd`**-style settings (no profile switches).

**Updates:** Build an update bundle with `make package-update` or `pwsh ./scripts/Package-Update.ps1` (output: `dist/motoklub-update`, no `data/` folder). Stop the app, copy files over the existing install, and **do not replace** `data/motoklub.db`. Restart `Start-Motoklub.ps1`; new migrations apply on startup. If you customized `appsettings.json` on site, merge changes instead of overwriting blindly.

Two-terminal dev mode from the repo:

```powershell
pwsh ./scripts/Start-Motoklub.ps1 -Dev
```
