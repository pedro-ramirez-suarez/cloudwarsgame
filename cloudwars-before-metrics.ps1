# CloudWars pilot — BEFORE metrics (Windows, ~30 min, run once before any change)
# Usage (PowerShell, from the CloudWars repo root, VS Developer PowerShell so msbuild is on PATH):
#   powershell -ExecutionPolicy Bypass -File cloudwars-before-metrics.ps1
# Output: .\pilot-before\ (checked into the pilot branch as the baseline the audit pack cites).
$ErrorActionPreference = "Continue"
$out = Join-Path (Get-Location) "pilot-before"
New-Item -ItemType Directory -Force -Path $out | Out-Null
$stamp = Get-Date -Format "yyyy-MM-ddTHH-mm-ss"
"CloudWars before-metrics $stamp" | Tee-Object -FilePath (Join-Path $out "README.txt")

# 1. Environment fingerprint (C2: behavior baselines carry one)
@{
  timestamp = $stamp
  os = (Get-CimInstance Win32_OperatingSystem).Caption
  msbuild = (& msbuild -version 2>$null | Select-Object -Last 1)
  dotnet = (& dotnet --version 2>$null)
  git = (& git rev-parse HEAD 2>$null)
  gitStatusDirty = ((& git status --porcelain 2>$null | Measure-Object).Count)
} | ConvertTo-Json | Set-Content (Join-Path $out "environment.json")

# 2. Baseline build state of the UNTOUCHED solution (which projects compile, warning count)
$sln = Get-ChildItem -Filter *.sln | Select-Object -First 1
& msbuild $sln.FullName /t:Restore,Build /p:Configuration=Debug /nologo /v:m /clp:Summary 2>&1 | Tee-Object -FilePath (Join-Path $out "msbuild-baseline.log") | Out-Null
$log = Get-Content (Join-Path $out "msbuild-baseline.log")
@{
  errors = ($log | Select-String -Pattern "error [A-Z]+\d+" | Measure-Object).Count
  warnings = ($log | Select-String -Pattern "warning [A-Z]+\d+" | Measure-Object).Count
  succeeded = [bool]($log | Select-String -Pattern "Build succeeded")
} | ConvertTo-Json | Set-Content (Join-Path $out "build-summary.json")

# 3. Target frameworks and project styles (before/after metric #1)
Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
  $x = [xml](Get-Content $_.FullName)
  [pscustomobject]@{
    project = $_.Name
    sdkStyle = [bool]$x.Project.Sdk
    targetFramework = ($x.Project.PropertyGroup | ForEach-Object { $_.TargetFrameworkVersion; $_.TargetFramework } | Where-Object { $_ } | Select-Object -First 1)
    lines = (Get-ChildItem $_.DirectoryName -Recurse -Filter *.cs | Get-Content | Measure-Object -Line).Lines
  }
} | ConvertTo-Json | Set-Content (Join-Path $out "projects.json")

# 4. Needletail API call inventory (Phase 2 surface; checked in)
Get-ChildItem -Recurse -Filter *.cs | Select-String -Pattern "Needletail|CloudWarsDB|\.Query<|\.Execute\(|\.Get<|\.Save\(|\.Insert\(|\.Update\(|\.Delete\(" |
  ForEach-Object { "$($_.Path):$($_.LineNumber): $($_.Line.Trim())" } | Set-Content (Join-Path $out "needletail-api-calls.txt")
(Get-Content (Join-Path $out "needletail-api-calls.txt") | Measure-Object).Count | Set-Content (Join-Path $out "needletail-api-calls.count")

# 5. Grep-countable legacy idioms (fixed now; re-counted after each phase)
$idioms = @{
  "static-gateway"      = "CloudWarsDB\."
  "config-appsettings"  = "ConfigurationManager\.AppSettings"
  "web-config"          = "WebConfigurationManager|<connectionStrings>"
  "sync-over-async"     = "\.Result\b|\.Wait\(\)"
  "datatable"           = "DataTable|DataSet|SqlDataReader"
  "region-blocks"       = "#region"
  "hungarian-fields"    = "\b(str|int|bln|obj)[A-Z][A-Za-z]+\b"
  "legacy-framework-ns" = "System\.Web\.|Microsoft\.WindowsAzure"
}
$counts = @{}
foreach ($k in $idioms.Keys) {
  $counts[$k] = (Get-ChildItem -Recurse -Filter *.cs | Select-String -Pattern $idioms[$k] | Measure-Object).Count
}
$counts | ConvertTo-Json | Set-Content (Join-Path $out "idiom-counts.json")

# 6. Tests present today (expected: none)
(Get-ChildItem -Recurse -Filter *.cs | Select-String -Pattern "\[TestMethod\]|\[Fact\]|\[Test\]" | Measure-Object).Count | Set-Content (Join-Path $out "test-count.before")

"Done. Review pilot-before\ and commit it on the pilot branch." | Tee-Object -FilePath (Join-Path $out "README.txt") -Append
