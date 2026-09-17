# CloudWars pilot — metrics snapshot (Windows, VS Developer PowerShell so msbuild is on PATH)
# Usage, from the CloudWars repo root:
#   before (once, on the untouched tree):  powershell -ExecutionPolicy Bypass -File cloudwars-before-metrics.ps1
#   after  (on the pilot branch):          powershell -ExecutionPolicy Bypass -File cloudwars-before-metrics.ps1 -Out pilot-after
# Output: .\<Out>\ (checked into the pilot branch; the audit pack cites before and after side by side).
# 2026-09-17 revision after the first after-run: -Out parameter; multi-target projects report every
# target (the first version wrote {} for <TargetFrameworks>); idiom counts are case-sensitive (the
# case-insensitive default counted "string" and "object" as Hungarian fields); the static-data-facade
# idiom (CloudWarsData.) is counted beside the gateway (CloudWarsDB.) because the module calls the
# facade, not the gateway; counts are also written per project so tests and module separate;
# inventory paths are repo-relative so runs on different machines diff cleanly.
param([string]$Out = "pilot-before")
$ErrorActionPreference = "Continue"
$root = (Get-Location).Path
$out = Join-Path $root $Out
New-Item -ItemType Directory -Force -Path $out | Out-Null
$stamp = Get-Date -Format "yyyy-MM-ddTHH-mm-ss"
"CloudWars metrics ($Out) $stamp" | Tee-Object -FilePath (Join-Path $out "README.txt")

# 1. Environment fingerprint (C2: behavior baselines carry one)
@{
  timestamp = $stamp
  os = [string](Get-CimInstance Win32_OperatingSystem).Caption
  msbuild = [string](& msbuild -version 2>$null | Select-Object -Last 1)
  dotnet = [string](& dotnet --version 2>$null)
  git = [string](& git rev-parse HEAD 2>$null)
  gitStatusDirty = ((& git status --porcelain 2>$null | Measure-Object).Count)
} | ConvertTo-Json | Set-Content (Join-Path $out "environment.json")

# 2. Build state of the solution as checked in (which projects compile, warning count)
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
  $targets = $x.SelectNodes("//*[local-name()='TargetFrameworkVersion' or local-name()='TargetFramework' or local-name()='TargetFrameworks']") | ForEach-Object { $_.InnerText }
  [pscustomobject]@{
    project = $_.Name
    sdkStyle = [bool]$x.Project.Sdk
    targetFramework = [string]($targets -join ",")
    lines = (Get-ChildItem $_.DirectoryName -Recurse -Filter *.cs | Get-Content | Measure-Object -Line).Lines
  }
} | ConvertTo-Json | Set-Content (Join-Path $out "projects.json")

# 4. Needletail API call inventory (Phase 2 surface; checked in), repo-relative paths
Get-ChildItem -Recurse -Filter *.cs | Select-String -CaseSensitive -Pattern "Needletail|CloudWarsDB|\.Query<|\.Execute\(|\.Get<|\.Save\(|\.Insert\(|\.Update\(|\.Delete\(" |
  ForEach-Object { "$($_.Path.Substring($root.Length + 1)):$($_.LineNumber): $($_.Line.Trim())" } | Set-Content (Join-Path $out "needletail-api-calls.txt")
(Get-Content (Join-Path $out "needletail-api-calls.txt") | Measure-Object).Count | Set-Content (Join-Path $out "needletail-api-calls.count")

# 5. Grep-countable legacy idioms (case-sensitive; totals and per top-level project folder)
$idioms = @{
  "static-gateway"      = "CloudWarsDB\."
  "static-data-facade"  = "CloudWarsData\."
  "config-appsettings"  = "ConfigurationManager\.AppSettings"
  "web-config"          = "WebConfigurationManager|<connectionStrings>"
  "sync-over-async"     = "\.Result\b|\.Wait\(\)"
  "datatable"           = "DataTable|DataSet|SqlDataReader"
  "region-blocks"       = "#region"
  "hungarian-fields"    = "\b(str|int|bln|obj)[A-Z][A-Za-z]+\b"
  "legacy-framework-ns" = "System\.Web\.|Microsoft\.WindowsAzure"
}
$counts = @{}
$byProject = @{}
foreach ($k in $idioms.Keys) {
  $hits = @(Get-ChildItem -Recurse -Filter *.cs | Select-String -CaseSensitive -Pattern $idioms[$k])
  $counts[$k] = $hits.Count
  $per = @{}
  foreach ($h in $hits) {
    $p = ($h.Path.Substring($root.Length + 1) -split "[\\/]")[0]
    $per[$p] = 1 + [int]$per[$p]
  }
  $byProject[$k] = $per
}
$counts | ConvertTo-Json | Set-Content (Join-Path $out "idiom-counts.json")
$byProject | ConvertTo-Json -Depth 3 | Set-Content (Join-Path $out "idiom-counts-by-project.json")

# 6. Test methods present ([Fact]/[Theory]/[Test]/[TestMethod] attributes)
(Get-ChildItem -Recurse -Filter *.cs | Select-String -CaseSensitive -Pattern "\[TestMethod\]|\[Fact\]|\[Theory\]|\[Test\]" | Measure-Object).Count | Set-Content (Join-Path $out "test-count.before")

"Done. Review $Out\ and commit it on the pilot branch." | Tee-Object -FilePath (Join-Path $out "README.txt") -Append
