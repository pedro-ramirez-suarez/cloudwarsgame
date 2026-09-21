## Options

Estimates: CloudWars pilot, 2026-09-17: 8 landed increments (4 pin, 4 transform) in 24 sessions, 59 model calls, 10.9 h elapsed; rates fitted on 1,467 recorded calls (R² 0.95).

How to read "Fit": **yes** means the code and the client's intake both call for it; **possible, not indicated** means it is always available but nothing in the code or the intake asked for it — it keeps its estimate as the comparison, and the row says what would make it a yes; **not assessed** means a survey cannot judge it. Intake rows marked *assumed* took a default, so a "not indicated" that rests on an assumed answer is a question for the client, not a verdict.

| Type | Fit | Why | Scope | Pins + transforms | Tool h | Human h | Elapsed (working days) | Confidence |
|---|---|---|---|---|---|---|---|---|
| T1 Platform upgrade in place | yes | 2 runtime(s) out of support and 1 ending within a year; 3 legacy-format project(s) | CloudWars.Characterization.Db, CloudWars.Characterization, CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, Cl… | 7 + 9 | 15.3 | 7.7 | 3–6 | low |
| T2 Upgrade and containerize | yes | follows T1; makes the upgraded build deployable anywhere | CloudWars.Characterization.Db, CloudWars.Characterization, CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, Cl… | 7 + 10 | 16.3 | 8.2 | 3–6 | low |
| T3 Cloud-optimize / re-platform | possible, not indicated | intake does not name a cloud target (hosting: undecided) | CloudWars.Characterization.Db, CloudWars.Characterization, CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, Cl… | 7 + 3 | 9.6 | 4.8 | 2–4 | low |
| T4 Partial rewrite by layer | possible, not indicated | fewer than two layer-shaped projects | CloudWars.DataAccess | 1 + 1 | 1.9 | 1 | 1–1 | low |
| T5 Module extraction / strangler | yes | 9 projects: seam-and-replace one module at a time (the pilot's method) | CloudWars.Characterization.Db, CloudWars.Common, CloudWars.DataAccess, CloudWars.Entities, CloudWars.Game, CloudWars.Sp… | 5 + 12 | 16.3 | 8.2 | 3–6 | low |
| T6 Full behaviour-preserving rewrite | possible, not indicated | always possible; not indicated because the intake's appetite is "upgrade in place where possible, rewrite only what blocks it" — the estimate below is the comp… | CloudWars.Characterization.Db, CloudWars.Characterization, CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, Cl… | 9 + 17 | 41.1 | 20.6 | 8–15 | low |
| T7 Re-architecture | possible, not indicated | always possible; not indicated because the goals name no scale, service-boundary or team-topology driver ("Keep the system supportable and changeable: supporte… | CloudWars.Characterization.Db, CloudWars.Characterization, CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, Cl… | 9 + 10 | 27.7 | 13.9 | 5–10 | low |
| T8 Language or stack migration | yes | CloudWars.Game: System.Web (20 lines) (ASP.NET MVC 4.0.20710.0, ASP.NET Web API 4.0.20710.0) | CloudWars.Game | 9 + 11 | 19.1 | 9.6 | 4–7 | low |
| T9 Data migration | possible, not indicated | data access via CloudWars.Characterization.Db: System.Data.SqlClient, CloudWars.Game: EntityFramework; no engine change requested | CloudWars.Characterization.Db, CloudWars.Game | 1 + 2 | 2.9 | 1.5 | 1–2 | low |
| T10 Dependency replacement | yes | CloudWars.Characterization.Db: System.Data.SqlClient (superseded by Microsoft.Data.SqlClient); CloudWars.Game: DotNetOpenAuth.AspNet (unmaintained since 2015);… | CloudWars.Characterization.Db, CloudWars.Game | 1 + 20 | 20.1 | 10.1 | 4–8 | low |
| T11 Lift-and-shift | possible, not indicated | either hosting stays or code changes are acceptable, so an upgrade beats a lift | - | 0 + 1 | 1 | 0.5 | 1–1 | low |
| T12 Retire and replace | not assessed | not assessed by the survey: a build-versus-buy decision needs the market, not the code | - | - | - | - | - | - |

### T1 Platform upgrade in place

Why it fits: 2 runtime(s) out of support and 1 ending within a year; 3 legacy-format project(s).

Scope: CloudWars.Characterization.Db, CloudWars.Characterization, CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, CloudWars.Entities, CloudWars.Game, CloudWars.Glue, CloudWars.SpaceBattle.

Changes: Framework, runtime and language version; project format; package versions. Preserves: Architecture, code, behaviour, data.

Risks:
- API removals between framework generations surface at build time; the pins catch behavioural drift
- Toolchain floor rises for every developer (pilot finding)

Preconditions:
- Characterization pins first for 7 project(s) without tests (CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, CloudWars.Entities, CloudWars.Game, CloudWars.Glue, …)
- Target: Latest long-term-support release of the current stack

Consequences:
- IDE and SDK floor rises; CI images change

Estimate basis: 16 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement.

### T2 Upgrade and containerize

Why it fits: follows T1; makes the upgraded build deployable anywhere.

Scope: CloudWars.Characterization.Db, CloudWars.Characterization, CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, CloudWars.Entities, CloudWars.Game, CloudWars.Glue, CloudWars.SpaceBattle.

Changes: T1 plus Dockerfile, compose, configuration from environment, health endpoints, logging to stdout. Preserves: Behaviour, data.

Risks:
- Configuration read from files or the registry must move to the environment (see config idioms in D13)

Preconditions:
- Characterization pins first for 7 project(s) without tests (CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, CloudWars.Entities, CloudWars.Game, CloudWars.Glue, …)

Consequences:
- Container registry, secrets handling
- Container runtime on developer machines

Estimate basis: 17 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement.

### T5 Module extraction / strangler

Why it fits: 9 projects: seam-and-replace one module at a time (the pilot's method).

Scope: CloudWars.Characterization.Db, CloudWars.Common, CloudWars.DataAccess, CloudWars.Entities, CloudWars.Game, CloudWars.SpaceBattle.

Changes: One module at a time behind a seam, rewritten or replaced. Preserves: Everything else, until its turn.

Risks:
- Static gateways and shared state resist seams (see idioms)

Preconditions:
- Characterization pins first for 7 project(s) without tests (CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, CloudWars.Entities, CloudWars.Game, CloudWars.Glue, …)

Consequences:
- Routing or facade infrastructure

Estimate basis: 17 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement.

### T8 Language or stack migration

Why it fits: CloudWars.Game: System.Web (20 lines) (ASP.NET MVC 4.0.20710.0, ASP.NET Web API 4.0.20710.0).

Scope: CloudWars.Game.

Changes: VB.NET to C#, AngularJS to a current framework, jQuery pages to components, Web Forms to MVC/Razor/Blazor. Preserves: Behaviour, structure where possible.

Risks:
- Translation drift per file; idiom conformance must be checked, not assumed

Preconditions:
- Characterization pins first for 7 project(s) without tests (CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, CloudWars.Entities, CloudWars.Game, CloudWars.Glue, …)

Consequences:
- New build chain

Estimate basis: 20 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); sized from the pricing units (31 actions, 1 EF contexts, 4 controllers, 11 tables, 29 views) with the rewrite weights (10 facts per pin increment; a controller with its views, a form, a context, a report, a contract or a third-party control package is one increment); code-creating increments at 1× the pilot's volumes (the pilot edited, it did not create: an assumption until a rewrite is calibrated); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement.

Sized from: 31 actions, 1 EF contexts, 4 controllers, 11 tables, 29 views.

### T10 Dependency replacement

Why it fits: CloudWars.Characterization.Db: System.Data.SqlClient (superseded by Microsoft.Data.SqlClient); CloudWars.Game: DotNetOpenAuth.AspNet (unmaintained since 2015); CloudWars.Game: DotNetOpenAuth.Core (unmaintained since 2015); CloudWars.Game: DotNetOpenAuth.OAuth.Consumer (unmaintained since 2015); CloudWars.Game: DotNetOpenAuth.OAuth.Core (unmaintained since 2015); CloudWars.Game: DotNetOpenAuth.OpenId.Core (unmaintained since 2015); CloudWars.Game: DotNetOpenAuth.OpenId.RelyingParty (unmaintained since 2015); CloudWars.Game: EntityFramework (EF6: maintenance only); CloudWars.Game: jQuery.UI.Combined (jQuery UI: maintenance only); CloudWars.Game: knockoutjs (Knockout: maintenance only); CloudWars.Game: Microsoft.AspNet.Mvc (ASP.NET MVC 5 and earlier: no longer serviced); CloudWars.Game: Microsoft.AspNet.Mvc.FixedDisplayModes (ASP.NET MVC 5 and earlier: no longer serviced); CloudWars.Game: Microsoft.AspNet.WebApi (ASP.NET Web API 2: no longer serviced); CloudWars.Game: Microsoft.AspNet.WebApi.Client (ASP.NET Web API 2: no longer serviced); CloudWars.Game: Microsoft.AspNet.WebApi.Core (ASP.NET Web API 2: no longer serviced); CloudWars.Game: Microsoft.AspNet.WebApi.OData (ASP.NET Web API 2: no longer serviced); CloudWars.Game: Microsoft.AspNet.WebApi.WebHost (ASP.NET Web API 2: no longer serviced); CloudWars.Game: Needletail.DataAccess (no maintained release); CloudWars.Game: Needletail.DataAccess.Migrations (no maintained release); CloudWars.Game: Needletail.Mvc (no maintained release).

Scope: CloudWars.Characterization.Db, CloudWars.Game.

Changes: An unsupported library replaced behind an adapter. Preserves: Everything else.

Risks:
- Replacement libraries differ in edge behaviour; adapter first, pins around the use sites

Preconditions:
- Characterization pins first for 7 project(s) without tests (CloudWars.Common, CloudWars.DataAccess, CloudWars.Engine, CloudWars.Entities, CloudWars.Game, CloudWars.Glue, …)

Consequences:
- Usually none

Estimate basis: 21 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement.

## Possible, not indicated

Available on any codebase; nothing here asked for them yet. Each keeps its estimate so the client can weigh it against the recommended path.

### T3 Cloud-optimize / re-platform

Why not now: intake does not name a cloud target (hosting: undecided).

What would make it a yes: Name a cloud in the hosting answer, or Kubernetes / serverless in orchestration; the estimate assumes a test environment the client provides.

Changes: Managed services, configuration and secrets, observability, Kubernetes manifests or Helm, autoscaling. Preserves: Business logic, most code. Pins and gates: Pins plus infrastructure tests (deploy to a test cluster, smoke); Smoke on the target platform.

If chosen: 7 pin + 3 transform increments ≈ 9.6 tool hours, 4.8 human hours, 2–4 working days (low confidence; 10 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement).

### T4 Partial rewrite by layer

Why not now: fewer than two layer-shaped projects.

What would make it a yes: Name the layer to replace under modules of interest (a UI, API or data-access project), with an appetite other than "upgrade in place only".

Changes: One layer replaced: UI, backend services, or data access. Preserves: The other layers and their contracts. Pins and gates: Contract tests at the layer boundary; Boundary pins green with the new layer in place.

If chosen: 1 pin + 1 transform increments ≈ 1.9 tool hours, 1 human hours, 1–1 working days (low confidence; 2 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement).

### T6 Full behaviour-preserving rewrite

Why not now: always possible; not indicated because the intake's appetite is "upgrade in place where possible, rewrite only what blocks it" — the estimate below is the comparison against the incremental path.

What would make it a yes: Set the rewrite appetite to "full rewrite preserving behaviour". Precondition: whole-system pins and golden data before any code is replaced; the estimate here is the comparison against the incremental path.

Changes: All code, possibly the stack. Preserves: Business rules, data, external contracts. Pins and gates: Whole-system pins (API, UI flows, data), golden data sets, parallel run; Parallel run parity.

If chosen: 9 pin + 17 transform increments ≈ 41.1 tool hours, 20.6 human hours, 8–15 working days (low confidence; 26 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); sized from the pricing units (31 actions, 1 EF contexts, 4 controllers, 11 tables, 29 views; plus 6 fixed increments) with the rewrite weights (10 facts per pin increment; a controller with its views, a form, a context, a report, a contract or a third-party control package is one increment); code-creating increments at 2× the pilot's volumes (the pilot edited, it did not create: an assumption until a rewrite is calibrated); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement).

### T7 Re-architecture

Why not now: always possible; not indicated because the goals name no scale, service-boundary or team-topology driver ("Keep the system supportable and changeable: supported runtimes, a test safety ne…").

What would make it a yes: State a scale, team-topology or service-boundary goal (services, event-driven, independent deployment). Nothing in a codebase makes this necessary on its own.

Changes: Monolith to services or modular monolith; synchronous to event-driven. Preserves: Business rules. Pins and gates: Contract pins per boundary; data ownership tests; Boundary contracts green.

If chosen: 9 pin + 10 transform increments ≈ 27.7 tool hours, 13.9 human hours, 5–10 working days (low confidence; 19 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); sized from the pricing units (31 actions, 1 EF contexts, 4 controllers, 11 tables, 29 views; plus 4 fixed increments) with the rewrite weights (10 facts per pin increment; a controller with its views, a form, a context, a report, a contract or a third-party control package is one increment); code-creating increments at 2× the pilot's volumes (the pilot edited, it did not create: an assumption until a rewrite is calibrated); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement).

### T9 Data migration

Why not now: data access via CloudWars.Characterization.Db: System.Data.SqlClient, CloudWars.Game: EntityFramework; no engine change requested.

What would make it a yes: A database package in the code plus a cloud target or a named engine in the target stack (for example SQL Server to PostgreSQL).

Changes: Database engine or schema; data cleanup. Preserves: Application behaviour. Pins and gates: Data-parity checks; dual-write or replay; Parity on a copy of production data.

If chosen: 1 pin + 2 transform increments ≈ 2.9 tool hours, 1.5 human hours, 1–2 working days (low confidence; 3 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement).

### T11 Lift-and-shift

Why not now: either hosting stays or code changes are acceptable, so an upgrade beats a lift.

What would make it a yes: A hosting change with the appetite "upgrade in place only": move the code as it is, prove it with smoke tests.

Changes: Infrastructure only. Preserves: Code. Pins and gates: Smoke tests; no code pins; Smoke on the new infrastructure.

If chosen: 0 pin + 1 transform increments ≈ 1 tool hours, 0.5 human hours, 1–1 working days (low confidence; 1 increments at the pilot's volumes (57 min of model time and 82 min elapsed each); human hours = tool hours × 0.5 (operator ratio: the pilot measured about 0.25, one engagement; 0.5 is the midpoint used until a second engagement measures it); confidence low: one calibration engagement).


### T12 Retire and replace — not assessed

not assessed by the survey: a build-versus-buy decision needs the market, not the code. Not a survey question: a build-versus-buy decision needs the market, the vendor and the data-export contract.

## Recommendation

Sequence: T10 → T1 → T2 → T5 → T8.

- Pin first: 7 of 9 projects have no tests, so no change can be shown safe until characterization tests exist.
- Dependency replacement removes unmaintained code paths before they are carried onto a new runtime.
- The in-place upgrade addresses the out-of-support runtimes with the least change to the code.
- Containerizing the upgraded build makes it deployable on any host and is the precondition for a cloud move.
- Where modules must be rewritten, the strangler order limits the dual-run exposure to one module at a time.
- Stack migration follows the upgrade so it targets the supported platform.
