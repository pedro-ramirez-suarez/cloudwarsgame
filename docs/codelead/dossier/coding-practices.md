# Coding practices & principles — pedro-ramirez-suarez-fluffy-system

Generated 2026-09-23T00:58:16.134Z · 91 source files scanned

This section reports only what makes the system **harder or riskier to change**.
It is not a style review: there is nothing here about formatting or naming, because
neither changes migration risk. Every row below has a direct line to "you cannot
safely transform this yet".

## What raises migration risk here

| Principle | Occurrences | Files | Why it matters for a migration |
|---|---|---|---|
| **Fail visibly** | 12 | 4 | A migration that breaks this path produces no error and no log line. The system reports success while doing the wrong thing — the single most expensive failure mode to discover late. |
| **Avoid shared mutable state** | 10 | 2 | Shared across every test in the process, so tests touching it cannot run in isolation or in parallel. Characterization tests — the first thing a migration needs — are the hardest to write against exactly this. |
| **Single responsibility — file size** | 2 | 2 | Every change to this file produces a diff no reviewer can fully verify, and the file will appear in the change surface of unrelated increments — the pattern that makes migrations stall. |
| **Single responsibility — method length** | 1 | 1 | Too long to characterize with a focused test, so it is usually migrated whole and verified by eye — the step where behaviour quietly changes. |
| **Dependency inversion** | 1 | 1 | This code cannot be exercised without the real dependency, so it cannot be pinned by a characterization test before the migration, and the dependency cannot be swapped for its replacement afterwards. |

## Evidence

### Fail visibly — 12 occurrence(s)

- `CloudWars.Characterization.Db/SpaceBattleClientFeedbackDbTests.cs:27` — An exception is caught and discarded without logging, rethrowing, or handling.
- `CloudWars.Characterization.Db/SpaceBattleClientFeedbackDbTests.cs:32` — An exception is caught and discarded without logging, rethrowing, or handling.
- `CloudWars.Characterization.Db/SpaceBattleClientFeedbackDbTests.cs:37` — An exception is caught and discarded without logging, rethrowing, or handling.
- … and 9 more

### Avoid shared mutable state — 10 occurrence(s)

- `CloudWars.DataAccess/Sql/CloudWarsDB.cs:19` — `_Matches` is mutable static state.
- `CloudWars.DataAccess/Sql/CloudWarsDB.cs:29` — `_MatchUnits` is mutable static state.
- `CloudWars.DataAccess/Sql/CloudWarsDB.cs:39` — `_PlayerUnits` is mutable static state.
- … and 7 more

### Single responsibility — file size — 2 occurrence(s)

- `CloudWars.Game/Scripts/jquery.validate-vsdoc.js:1` — 1292 lines in one file.
- `CloudWars.Game/Scripts/jquery.validate.js:1` — 1249 lines in one file.

### Single responsibility — method length — 1 occurrence(s)

- `CloudWars.Engine/WorkerRole.cs:30` — `Run` is 88 lines long.

### Dependency inversion — 1 occurrence(s)

- `CloudWars.DataAccess/Queue/MessageRepositoryBase.cs:39` — StreamReader is constructed directly here rather than injected.

## Concentration by area

| Area | Findings |
|---|---|
| `CloudWars.Characterization.Db` | 12 |
| `CloudWars.DataAccess` | 8 |
| `CloudWars.Game` | 5 |
| `CloudWars.Engine` | 1 |

Areas at the top are where a migration should expect to spend refactoring effort
*before* transformation, not during it.

> Only patterns that raise migration risk are reported. A pattern's absence is not a claim that the code is idiomatic — it means this pass found nothing that would make a transformation more dangerous.
