# Portable build (requires pwsh, .NET 8 SDK, Node). From repo root: make package

.PHONY: package package-fast package-self-contained package-self-contained-fast package-update clean

package:
	pwsh -NoProfile -File ./scripts/Package-Local.ps1

package-fast:
	pwsh -NoProfile -File ./scripts/Package-Local.ps1 -Fast

package-self-contained:
	pwsh -NoProfile -File ./scripts/Package-Local.ps1 -SelfContained -Runtime win-x64

package-self-contained-fast:
	pwsh -NoProfile -File ./scripts/Package-Local.ps1 -SelfContained -Runtime win-x64 -Fast

package-update:
	pwsh -NoProfile -File ./scripts/Package-Update.ps1

clean:
	pwsh -NoProfile -Command "Remove-Item -Recurse -Force -Path 'dist/motoklub-local','dist/motoklub-update' -ErrorAction SilentlyContinue"
