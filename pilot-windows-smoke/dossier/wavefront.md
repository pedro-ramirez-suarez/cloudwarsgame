# Modernization candidates & wavefront — cloudwarsgame

Generated 2026-09-21T20:44:06.468Z · project index: rebuilt

## Clusters (what moves together)

| Cluster | Modules | Internal edges | Crossing edges | Isolation |
|---|---|---|---|---|
| 1 | `codelead` | 0 | 0 | 1.00 |
| 2 | `pilot-after` | 0 | 0 | 1.00 |
| 3 | `(root)` | 0 | 0 | 1.00 |
| 4 | `pilot-before` | 0 | 0 | 1.00 |
| 5 | `CloudWars.Glue` | 0 | 0 | 1.00 |
| 0 | `CloudWars.Characterization`, `CloudWars.Characterization.Db`, `CloudWars.Common`, `CloudWars.DataAccess`, `CloudWars.Engine`, `CloudWars.Entities`, `CloudWars.Game`, `CloudWars.SpaceBattle` | 72 | 9 | 0.89 |

## Proposed waves (5 waves)

| Wave | Module | Depends on | Depended on by | Why |
|---|---|---|---|---|
| 1 | `(root)` | 0 | 0 | nothing depends on it — safe to migrate in isolation |
| 1 | `CloudWars.Characterization` | 3 | 0 | nothing depends on it — safe to migrate in isolation |
| 1 | `CloudWars.Characterization.Db` | 4 | 0 | nothing depends on it — safe to migrate in isolation |
| 1 | `CloudWars.Engine` | 3 | 0 | nothing depends on it — safe to migrate in isolation |
| 1 | `CloudWars.Game` | 4 | 0 | nothing depends on it — safe to migrate in isolation |
| 1 | `CloudWars.Glue` | 0 | 0 | nothing depends on it — safe to migrate in isolation |
| 1 | `codelead` | 0 | 0 | nothing depends on it — safe to migrate in isolation |
| 1 | `pilot-after` | 0 | 0 | nothing depends on it — safe to migrate in isolation |
| 1 | `pilot-before` | 0 | 0 | nothing depends on it — safe to migrate in isolation |
| 2 | `CloudWars.SpaceBattle` | 3 | 3 | its dependents are already migrated in earlier waves |
| 3 | `CloudWars.DataAccess` | 1 | 4 | its dependents are already migrated in earlier waves |
| 4 | `CloudWars.Entities` | 1 | 6 | its dependents are already migrated in earlier waves |
| 5 | `CloudWars.Common` | 0 | 6 | its dependents are already migrated in earlier waves |

> Clusters and wave order are a PROPOSAL derived from the dependency graph. The migration recommendation — what actually moves, in what order, at what risk — is a human judgment informed by this, the risk map (D6) and history (D4).
