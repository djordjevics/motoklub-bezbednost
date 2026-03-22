# Builds a portable folder: published API + wwwroot (Vite) + Start-Motoklub.ps1.
# Usage (from repo root):  pwsh ./scripts/Package-Local.ps1
# Optional: -SelfContained -Runtime win-x64   (no .NET runtime needed on target machine; larger output)
# Optional: -SkipDataLayout   (omit data/ - used by Package-Update.ps1 for copy-over upgrades)

param(
    [string]$OutputRelative = "dist/motoklub-local",
    [switch]$SelfContained,
    [string]$Runtime = "win-x64",
    [switch]$SkipDataLayout
)

$ErrorActionPreference = "Stop"
$RepoRoot = Split-Path $PSScriptRoot -Parent
$Output = Join-Path $RepoRoot $OutputRelative
$BackendApi = Join-Path $RepoRoot "backend/MotoklubBezbednost.API/MotoklubBezbednost.API.csproj"
$Frontend = Join-Path $RepoRoot "frontend"

if (-not (Test-Path $BackendApi)) { throw "Backend project not found: $BackendApi" }
if (-not (Test-Path $Frontend)) { throw "Frontend folder not found: $Frontend" }

$publishArgs = @(
    "publish", $BackendApi,
    "-c", "Release",
    "-o", $Output
)
if ($SelfContained) {
    $publishArgs += @("--self-contained", "true", "-r", $Runtime)
}

Write-Host "Publishing API to $Output ..."
& dotnet @publishArgs

Push-Location $Frontend
try {
    $prev = $env:VITE_API_BASE_URL
    $env:VITE_API_BASE_URL = "/api"
    if (Test-Path "package-lock.json") {
        npm ci
    } else {
        npm install
    }
    npm run build
    if ($null -ne $prev) { $env:VITE_API_BASE_URL = $prev } else { Remove-Item Env:VITE_API_BASE_URL -ErrorAction SilentlyContinue }
}
finally {
    Pop-Location
}

$wwwroot = Join-Path $Output "wwwroot"
New-Item -ItemType Directory -Force -Path $wwwroot | Out-Null
Copy-Item -Path (Join-Path $Frontend "dist\*") -Destination $wwwroot -Recurse -Force

if (-not $SkipDataLayout) {
    $data = Join-Path $Output "data"
    $backups = Join-Path $data "backups"
    New-Item -ItemType Directory -Force -Path $backups | Out-Null
}

Copy-Item (Join-Path $RepoRoot "scripts/Start-Motoklub.ps1") $Output -Force

if ($SkipDataLayout) {
    Write-Host "Done (update bundle: no data folder; keep existing deployment database files when copying over)."
} else {
    Write-Host "Done. Run (from repo root):"
    Write-Host "  pwsh -File `"$Output/Start-Motoklub.ps1`""
    Write-Host "Or cd into the folder, then: .\Start-Motoklub.ps1"
}
