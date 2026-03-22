# Same as Package-Local.ps1 but outputs to dist/motoklub-update and omits data/ (for copy-over upgrades).
# Recipient: stop app, merge this folder over the existing install without replacing data/*.db, then start again.

param(
    [switch]$SelfContained,
    [string]$Runtime = "win-x64"
)

$ErrorActionPreference = "Stop"
$script = Join-Path $PSScriptRoot "Package-Local.ps1"
& $script -OutputRelative "dist/motoklub-update" -SkipDataLayout @PSBoundParameters
Write-Host ""
Write-Host "Update: copy contents onto the existing deployment folder. Do not overwrite data/*.db. Restart Start-Motoklub.ps1."
