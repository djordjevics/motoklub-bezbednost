# Portable build (requires pwsh, .NET 8 SDK, Node). From repo root: make package

.PHONY: package package-self-contained package-update clean

package:
	pwsh -NoProfile -File ./scripts/Package-Local.ps1

package-self-contained:
	pwsh -NoProfile -File ./scripts/Package-Local.ps1 -SelfContained -Runtime win-x64

package-update:
	pwsh -NoProfile -File ./scripts/Package-Update.ps1

clean:
	pwsh -NoProfile -Command "Remove-Item -Recurse -Force -Path 'dist/motoklub-local','dist/motoklub-update' -ErrorAction SilentlyContinue"
