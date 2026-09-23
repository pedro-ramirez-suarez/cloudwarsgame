# Improvement & modernization opportunities — pedro-ramirez-suarez-fluffy-system

Generated 2026-09-23T00:58:23.046Z · 14 opportunities, ordered by consequence

Every item below is derived from a recorded finding — nothing here is taste.
**Mechanical** items are safe and repeatable once tests exist; **judgment** items need a decision.

| # | Opportunity | Category | Effort | Nature |
|---|---|---|---|---|
| O1 | Move 7 projects off an end-of-life runtime | runtime | weeks | judgment |
| O2 | Pin behaviour in 3 untested areas | verification | weeks | judgment |
| O3 | Resolve the legacy package manifest against a live advisory feed | dependencies | days | mechanical |
| O4 | Capture what Cloudancy knows | knowledge | days | judgment |
| O5 | Entity Framework 6 or earlier → Entity Framework Core | dependencies | months | judgment |
| O6 | ASP.NET MVC (System.Web) → ASP.NET Core MVC | dependencies | months | judgment |
| O7 | ASP.NET Web API 2 → ASP.NET Core controllers | dependencies | months | judgment |
| O8 | Extract seams in CloudWars.Game | structure | days | judgment |
| O9 | Plan the exit from 6 maintenance-only targets | runtime | weeks | judgment |
| O10 | Confirm the version policy for Needletail.DataAccess | dependencies | hours | judgment |
| O11 | Triage 4 author notes left in the code | hygiene | days | judgment |
| O12 | DotNetOpenAuth → a maintained OpenID Connect library | dependencies | days | judgment |
| O13 | jQuery 1.x → a current front-end approach | dependencies | days | judgment |
| O14 | Newtonsoft.Json → System.Text.Json | dependencies | days | judgment |

## Detail

### O1. Move 7 projects off an end-of-life runtime

**Finding.** CloudWars.Common/CloudWars.Common.csproj (net45), CloudWars.DataAccess/CloudWars.DataAccess.csproj (net45), CloudWars.Engine/CloudWars.Engine.csproj (net40), CloudWars.Entities/CloudWars.Entities.csproj (net45), CloudWars.Game/CloudWars.Game.csproj (net45), CloudWars.Glue/CloudWars.Glue.csproj (net40), CloudWars.SpaceBattle/CloudWars.SpaceBattle.csproj (net45) — end of support 2016-01-12 (target out of support; runs in place on 4.6.2+/4.8, which remain supported).

**Why it matters.** No vendor security fixes are available for these projects at all. A disclosed vulnerability has no remediation path other than in-house patching.

**What to do.** Retarget to a supported runtime. Do this after characterization tests exist, so the retarget is provable.

*Effort: weeks · judgment · evidence: dependencies.json → runtimes*

### O2. Pin behaviour in 3 untested areas

**Finding.** No test reaches CloudWars.Game, CloudWars.Engine, CloudWars.Glue.

**Why it matters.** No change to these areas — including a security patch — can be shown to be safe before release. This blocks every other improvement.

**What to do.** Start with the 2 area(s) rated "likely pinnable" (CloudWars.Engine, CloudWars.Glue); they need no refactoring first.

*Effort: weeks · judgment · evidence: test-census.json → testDeserts*

### O3. Resolve the legacy package manifest against a live advisory feed

**Finding.** Dependencies are declared in pre-SDK `packages.config` manifests, which the standard tooling cannot check for currency or advisories.

**Why it matters.** The security posture of these dependencies is unknown — not clean. "0 advisories" here means "not checked".

**What to do.** Resolve every pinned version against nuget.org (or an offline mirror), then migrate the manifests to PackageReference so future checks are automatic.

*Effort: days · mechanical · evidence: dependencies.json → notes*

### O4. Capture what Cloudancy knows

**Finding.** 2 core file(s) are ≥80% one author's work (CloudWars.Game/Web.config 100%; CloudWars.Game/CloudWars.Game.csproj 83%).

**Why it matters.** If that person becomes unavailable, the reasoning behind these files is unrecoverable — and it is the reasoning, not the code, that migrations depend on.

**What to do.** Interview now and record the answers as durable knowledge entries. This is the cheapest and most perishable item in any plan.

*Effort: days · judgment · evidence: git-history.json → authorConcentration*

### O5. Entity Framework 6 or earlier → Entity Framework Core

**Finding.** EntityFramework 5.0.0 is referenced by CloudWars.Game/packages.config.

**Why it matters.** requires a data-layer migration; usually the largest single item

**What to do.** Plan the replacement as part of the runtime move; requires a data-layer migration; usually the largest single item.

*Effort: months · judgment · evidence: dependencies.json → packages*

### O6. ASP.NET MVC (System.Web) → ASP.NET Core MVC

**Finding.** Microsoft.AspNet.Mvc 4.0.20710.0 is referenced by CloudWars.Game/packages.config.

**Why it matters.** the web host and pipeline change; controllers largely survive

**What to do.** Plan the replacement as part of the runtime move; the web host and pipeline change; controllers largely survive.

*Effort: months · judgment · evidence: dependencies.json → packages*

### O7. ASP.NET Web API 2 → ASP.NET Core controllers

**Finding.** Microsoft.AspNet.WebApi 4.0.20710.0 is referenced by CloudWars.Game/packages.config.

**Why it matters.** route and formatter configuration is rewritten

**What to do.** Plan the replacement as part of the runtime move; route and formatter configuration is rewritten.

*Effort: months · judgment · evidence: dependencies.json → packages*

### O8. Extract seams in CloudWars.Game

**Finding.** 61 I/O touch points across 38 source files (database 1, network 11, time 44, globals 5)

**Why it matters.** Time, network and database calls made inline cannot be substituted in a test, so this area's behaviour cannot be pinned as written.

**What to do.** Introduce injection points for the clock and external calls — the minimum change that makes the area testable, done before any behaviour change.

*Effort: days · judgment · evidence: test-census.json → areas*

### O9. Plan the exit from 6 maintenance-only targets

**Finding.** net8.0 receive security fixes only, with no new platform work.

**Why it matters.** Every year on a maintenance target widens the gap to the supported platform and raises the eventual cost of the move.

**What to do.** Schedule the upgrade as a funded phase rather than letting it arrive as an incident.

*Effort: weeks · judgment · evidence: dependencies.json → runtimes*

### O10. Confirm the version policy for Needletail.DataAccess

**Finding.** Needletail.DataAccess is used in 3 areas (3 references).

**Why it matters.** A shared component pinned to an old version means every fix made upstream has to be re-made here, or is simply missing.

**What to do.** Compare the pinned version against the component's current release and decide, explicitly, whether to track it.

*Effort: hours · judgment · evidence: usage.json → libraries*

### O11. Triage 4 author notes left in the code

**Finding.** 4 TODO/HACK/NOTE markers remain in the source.

**Why it matters.** Each is a note from an author to a future maintainer about work they knew was unfinished. A static survey cannot tell which were resolved elsewhere.

**What to do.** Triage them with the authors: resolve in code, or record an explicit decision to accept each one.

*Effort: days · judgment · evidence: business-rules.json → rules[kind=marker_comment]*

### O12. DotNetOpenAuth → a maintained OpenID Connect library

**Finding.** DotNetOpenAuth.AspNet 4.1.4.12333 is referenced by CloudWars.Game/packages.config.

**Why it matters.** the project is unmaintained; auth flows must be re-verified

**What to do.** Plan the replacement as part of the runtime move; the project is unmaintained; auth flows must be re-verified.

*Effort: days · judgment · evidence: dependencies.json → packages*

### O13. jQuery 1.x → a current front-end approach

**Finding.** jQuery 1.9.1 is referenced by CloudWars.Game/packages.config.

**Why it matters.** front-end work, independent of the server migration

**What to do.** Plan the replacement as part of the runtime move; front-end work, independent of the server migration.

*Effort: days · judgment · evidence: dependencies.json → packages*

### O14. Newtonsoft.Json → System.Text.Json

**Finding.** Newtonsoft.Json 4.5.11 is referenced by CloudWars.Game/packages.config.

**Why it matters.** mechanical for simple payloads; attribute-heavy models need review

**What to do.** Plan the replacement as part of the runtime move; mechanical for simple payloads; attribute-heavy models need review.

*Effort: days · judgment · evidence: dependencies.json → packages*
