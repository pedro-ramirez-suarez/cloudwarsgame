# Modernization readiness — pedro-ramirez-suarez-fluffy-system

Generated 2026-09-23 at `62bde2e`; history %as → %as (47 commits). 9 projects, 97 source files, 6,022 lines, 7 test files.

### Runtimes and frameworks

Support status from the end-of-life table: shipped snapshot 2026-09-18.

| Runtime | Status | Where |
|---|---|---|
| .NET Framework 4.0 | out of support since 2016-01-12 (10.7 years) | CloudWars.Engine/CloudWars.Engine.csproj: v4.0 |
| .NET Framework 4.5 | out of support since 2016-01-12 (10.7 years) | CloudWars.Common/CloudWars.Common.csproj: net45 |
| .NET 8.0 | support ends 2026-11-10 (2 months) | CloudWars.Characterization.Db/CloudWars.Characterization.Db.csproj: net8.0 |
| .NET Framework 4.8 | supported | CloudWars.Characterization.Db/CloudWars.Characterization.Db.csproj: net48 |

### Projects

| Project | Stack | Format | Targets | Frameworks | Source files | Lines | Tests | Last change | Idioms |
|---|---|---|---|---|---|---|---|---|---|
| CloudWars.Characterization.Db | dotnet | sdk | net48, net8.0 | - | 6 | 1,056 | 5 | %as | - |
| CloudWars.Characterization | dotnet | sdk | net8.0 | - | 2 | 181 | 2 | %as | - |
| CloudWars.Common | dotnet | sdk | net45, net8.0 | - | 10 | 242 | 0 | %as | - |
| CloudWars.DataAccess | dotnet | sdk | net45, net8.0 | - | 8 | 611 | 0 | %as | - |
| CloudWars.Engine | dotnet | legacy | v4.0 | WCF (System.ServiceModel) | 2 | 204 | 0 | %as | - |
| CloudWars.Entities | dotnet | sdk | net45, net8.0 | - | 9 | 191 | 0 | %as | - |
| CloudWars.Game | dotnet | legacy | v4.5 | ASP.NET MVC 4.0.20710.0; ASP.NET Web API 4.0.20710.0; Entity Framework 5.0.0 | 52 | 2,989 | 0 | %as | system-web 20, region-blocks 2 |
| CloudWars.Glue | dotnet | legacy | v4.0 | - | 2 | 47 | 0 | %as | - |
| CloudWars.SpaceBattle | dotnet | sdk | net45, net8.0 | - | 6 | 501 | 0 | %as | - |

### Pricing units

What modernization work is quoted per (pages, controllers, services, forms, reports, stored procedures), counted from files. The rule for each count is stated; a unit that is absent from the tree is not listed.

| Unit | Total | Per project | Counted as | Why it prices the work |
|---|---|---|---|---|
| MVC controllers | 4 | CloudWars.Game 4 | classes deriving from Controller | one per screen group; the unit of an MVC-to-ASP.NET Core move |
| Controller actions | 31 | CloudWars.Game 31 | public methods returning an action result in controller files | the endpoint count; scenarios and HTTP pins are written per action |
| Views and Razor components | 29 | CloudWars.Game 29 | `.cshtml`, `.vbhtml`, `.razor`, and `.aspx` under a Views folder | each view is re-rendered on the new host and compared |
| Entity Framework contexts | 1 | CloudWars.Game 1 | classes deriving from DbContext, ObjectContext or IdentityDbContext | each context is an EF6-to-EF Core boundary |
| Tables in SQL scripts | 11 | CloudWars.Game 11 | CREATE TABLE in `.sql` files | the data-and-outputs layer of the inventory |

### Build floor (as the tree is today)

- 3 legacy-format .NET projects: builds with MSBuild/Visual Studio on Windows (Mono reference assemblies elsewhere); `dotnet build` needs the SDK-style format
- 6 SDK-style .NET projects: Visual Studio 2017+ or the .NET SDK on any platform
- targets .NET Framework: needs the matching targeting pack (or the reference-assemblies package on non-Windows)

### Notes

- 7 of 9 projects have no test files: characterization tests come first (pin before transform).
