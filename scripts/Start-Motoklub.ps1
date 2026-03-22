# Backup SQLite, auto-migrate, run API (+ browser). Single DB: .\data\motoklub.db (prod-style settings).
#
# Packaged (script next to MotoklubBezbednost.API.dll):
#   .\Start-Motoklub.ps1
#
# Repo dev (this file under ./scripts): API + Vite in two windows
#   pwsh ./scripts/Start-Motoklub.ps1 -Dev

param(
    [switch]$Dev
)

$ErrorActionPreference = "Stop"

# Start-Process needs an executable path; "pwsh" is missing if only Windows PowerShell 5.1 is installed.
$pwshCmd = Get-Command pwsh.exe -ErrorAction SilentlyContinue
if (-not $pwshCmd) { $pwshCmd = Get-Command pwsh -ErrorAction SilentlyContinue }
$shellExe = if ($pwshCmd) { $pwshCmd.Source } else { $null }
if (-not $shellExe) {
    $shellExe = Join-Path $env:SystemRoot "System32\WindowsPowerShell\v1.0\powershell.exe"
}
if (-not (Test-Path -LiteralPath $shellExe)) {
    throw "PowerShell not found. Install PowerShell 7+ (pwsh) or ensure Windows PowerShell is present."
}

$env:ASPNETCORE_ENVIRONMENT = "LocalProd"
$env:Motoklub__AutoMigrate = "true"
$env:Motoklub__SqliteFileName = "motoklub.db"

if ($Dev) {
    $RepoRoot = Split-Path $PSScriptRoot -Parent
    $dataDir = Join-Path $RepoRoot "backend/MotoklubBezbednost.API/data"
    $backendDir = Join-Path $RepoRoot "backend/MotoklubBezbednost.API"
    $frontendDir = Join-Path $RepoRoot "frontend"
    $proj = Join-Path $backendDir "MotoklubBezbednost.API.csproj"

    if (-not (Test-Path $proj)) { throw "Use -Dev only when this script lives in <repo>/scripts (project not found)." }
}
else {
    $dataDir = Join-Path $PSScriptRoot "data"
    $backendDir = $PSScriptRoot
    $frontendDir = $null
    $proj = Join-Path $PSScriptRoot "MotoklubBezbednost.API.dll"
    if (-not (Test-Path $proj)) { throw "MotoklubBezbednost.API.dll not found. Run Package-Local.ps1 or start from publish folder." }
}

New-Item -ItemType Directory -Force -Path $dataDir | Out-Null
$backups = Join-Path $dataDir "backups"
New-Item -ItemType Directory -Force -Path $backups | Out-Null

$dbPath = Join-Path $dataDir "motoklub.db"
if (Test-Path $dbPath) {
    $stamp = Get-Date -Format "yyyyMMdd_HHmmss"
    Copy-Item $dbPath (Join-Path $backups "motoklub_${stamp}.db")
    Write-Host "Backed up database to backups folder."
}

$commonEnv = "`$env:ASPNETCORE_ENVIRONMENT='LocalProd'; `$env:Motoklub__AutoMigrate='true'; `$env:Motoklub__SqliteFileName='motoklub.db'"

if ($Dev) {
    $apiCmd = "Set-Location '$backendDir'; $commonEnv; dotnet run --project '$proj' --no-launch-profile --urls http://localhost:5000"
    Start-Process -FilePath $shellExe -ArgumentList "-NoExit", "-Command", $apiCmd
    Start-Sleep -Seconds 4
    $feCmd = "Set-Location '$frontendDir'; npm run dev"
    Start-Process -FilePath $shellExe -ArgumentList "-NoExit", "-Command", $feCmd
    Start-Sleep -Seconds 2
    Start-Process "http://localhost:3000"
    Write-Host "API: http://localhost:5000   UI (Vite): http://localhost:3000   DB: $dataDir\motoklub.db"
}
else {
    $apiCmd = "Set-Location '$backendDir'; $commonEnv; dotnet '$proj' --urls http://localhost:5000"
    Start-Process -FilePath $shellExe -ArgumentList "-NoExit", "-Command", $apiCmd
    Start-Sleep -Seconds 2
    Start-Process "http://localhost:5000"
    Write-Host "Single host (API + static UI): http://localhost:5000   DB: $dataDir\motoklub.db"
}
