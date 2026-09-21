# Windows smoke — results (wave 1.1, 2026-09-21, VM clock)

CodeLead running in the Windows VM against LM Studio on the Mac, following
`docs/windows-smoke-2026-09-22.md` in the CodeLead repository. Everything below was measured on
the VM; nothing was fixed by hand in the code CodeLead produced.

**Bottom line:** T1–T5 green on Windows after one CodeLead fix (the survey's sub-report spawns,
CodeLead `e90a5c2`) and one missing local file on a fresh clone (fixed for good on `pilot`,
`3fa940a`). `/implement` landed a test-only change first time: plan → patch → check → apply →
root gate 6/6 green in about 13 minutes, retries 0. The one red result after the fixes (a
120 s timeout on the Db build in T3) did not recur in three clean re-runs.

## Setup

| Item | Value |
|---|---|
| VM | Windows 10 Pro 19045, PowerShell 5.1, Git 2.16 |
| Node / npm | 24.19.0 / 11.17 (winget `OpenJS.NodeJS.LTS`) |
| .NET | SDK 10.0.401; runtimes 8.0.31 and 10.0.12; .NET Framework 4.8 reference assemblies; Visual Studio Community 2026 |
| CodeLead | `C:\projects\codelead\codelead` at `0042f44` (run 1), `0042f44` + the survey fix (run 2, committed as `e90a5c2`); `npm ci`, `npm run build`, `npm run typecheck` clean |
| Model | `qwen/qwen3.8-27b` on the Mac, `http://10.0.0.157:1234/v1` ("Serve on Local Network" on), reachable from the VM |
| Pilot | fresh clone of `pilot` at `6539ab3` into `C:\src\cloudwarsgame` |
| Environment | `CODELEAD_BASE_URL`, `CODELEAD_MODEL` as the smoke doc says; `CODELEAD_CHECKPOINT_COMMITS` **not** set (the doc did not list it then) |

**How it was driven.** The tests were run by a scripted driver, not typed by hand: the same
in-process pattern and prompt policy as `scripts/implementLiveRun.ts` (y/N prompts → yes,
numbered and press-Enter prompts → the default, anything else → the next command). One
consequence: the driver supplies no clarification prompt to `/modernize --plan`, so the intake
was not *asked*; it took every default, which is the same answer set as pressing Enter twelve
times, but the dossier records the intake source as "defaults" rather than "asked".

## Results by test

### T1 `/help` — green

The command list, `/modernize --plan` and `/session --export` among it. The CLI runs on Windows.

### T2 `/modernize --plan` — red in run 1, green in run 2 after a CodeLead fix

- **Run 1 (baseline):** `Surveying … (read-only; D1–D13)`, then every survey sub-report
  `FAILED —` with an empty reason; `Dossier bundle: docs\codelead\dossier/ (5 files)`;
  `modernization-readiness.json is missing, so no options were derived`; 0 knowledge entries.
- **Cause (two Windows bugs in `scripts/dossier/assembleDossier.ts`):** the script folder came
  from `new URL(import.meta.url).pathname`, which is `/C:/...` on Windows, so every
  sub-script path was wrong; and the inner `spawnSync("npx", …)` ran without a shell, which
  Node refuses for the `npx.cmd` shim (no exit status, no stderr — hence the blank reason). The
  2026-09-22 fix had shelled only the outer spawn in `planBundle.ts`.
- **Fix:** `fileURLToPath`, `shell: true` on win32, and a spawn error now prints its message.
- **Rescore, survey alone:** 10/10 sub-reports (D1 13 modules · D2 9 areas · D3 39 packages,
  14 runtime targets · D4 41 commits · D5 21 rules · D6 8 areas, 3 test deserts · D7 6
  clusters, 5 waves · D11 14 opportunities · D12 26 findings · D13 9 projects, 4 runtimes),
  41 knowledge entries (44 observations).
- **Run 2:** `Dossier bundle: docs\codelead\dossier/ (28 files)` and
  `Options: 5 of 12 fit (T1, T2, T5, T8, T10); recommended sequence: T10 → T1 → T2 → T5 → T8`
  — the Mac's result. 43 s end to end. The bundle is in `dossier/` here.

### T3 `/validate` (repo root scope) — red twice, then 3/3 green

The declared gate on Windows is six entries (Mono and the database pins are darwin/linux only):
builds of Common, Entities, DataAccess, SpaceBattle (both targets each), the in-memory pins
(`dotnet test`, net8.0), and the Db project's build (net48).

| Run | Result | Detail |
|---|---|---|
| Run 1 | failed | Entry 6: `MSB3030: Could not copy the file …\connections.local.config` (net48 and net8.0). The file is git-ignored and exists only on the Mac; the csproj copied it without the `Exists()` guard the line beside it has. A fresh clone cannot pass the gate. |
| — | VM workaround | A placeholder `connections.local.config` (git-ignored; `DefaultConnection` → `10.0.0.157,1433`, no password) so the project builds. |
| Run 2 | timed_out | Entries 1–5 green; entry 6 killed at the 120 s default with no output (`-v q` prints nothing until the end). A `npm run typecheck` was running on the VM at the same time. |
| T4's gate | passed | The same entry took 6.2 s about 15 minutes later. |
| Re-run ×3 (exclusive) | passed, passed, passed | Entry 6: 7.6 s, 8.1 s, 5.8 s. Whole gate 29–42 s per pass. |

**Conclusion:** the timeout was contention on the VM (two CPU-heavy jobs at once), not a code
fault. Lesson for the VM, as on the Mac: a gate run gets the machine to itself.

**Permanent fix on `pilot` (`3fa940a`):** `Condition="Exists('connections.local.config')"` on
the copy. Measured without the file: before 2 errors (MSB3030, both targets); after 0 warnings,
0 errors. The database pins still need a real connection file to *run* (smoke doc step 5).

### T4 `/implement` — landed

Request: *Add one more in-memory characterization fact to the CloudWars.Characterization
project, for a behaviour of SpaceBattleGame that the existing facts do not cover; test files
only; the root gate must stay green.*

| Stage | Time (UTC) | Duration | Detail |
|---|---|---|---|
| Start | 20:47:21 | | root gate red at the time (T3 run 2 timeout) |
| Preflight (model) | 20:47:21 → 20:54:01 | 6 m 40 s | prompt 30 KB, response 11 KB; clarifications took their defaults (exactly one in-memory fact, public behaviour only, no database-backed behaviour) |
| Patch (model) | 20:54:04 → 20:59:38 | 5 m 34 s | prompt 75 KB, response 2.5 KB |
| Check + apply | 20:59:38 → 20:59:40 | ~1 s | patch check passed first attempt; 1 file, 1 hunk |
| Validation | 20:59:40 → 21:00:16 | 36 s | repo root gate: 6/6 pass (3.3 s, 3.5 s, 4.8 s, 6.8 s, 11.8 s, 6.2 s) |
| Evidence | 21:00:16 | | `ev-20260921210016-e3zan4` (outcome `landed`, retries 0), copied to `evidence/` |

Total about 13 minutes, two model calls, against 20–45 minutes for this shape on the Mac.

**The change** (in `CloudWars.Characterization/SpaceBattleGameTests.cs`, where CodeLead put it,
so it runs in the gate): `ParameterlessConstructor_Defaults_MatchIdPlayersTurnAndReadyFlagsAreDefault`
— a new `SpaceBattleGame()` has `Guid.Empty` for `MatchId`, `Player1`, `Player2` and `Turn`, and
`false` for `Player1Ready` and `Player2Ready`. The existing in-memory facts covered `Units`
being null for this constructor and the two-parameter constructor's defaults, not these six
members on the parameterless one (the database pins were not checked for overlap).

**No checkpoint commit:** `/implement` commits only with `CODELEAD_CHECKPOINT_COMMITS=1`, which
the smoke doc omitted (corrected in CodeLead `e90a5c2`). The change was committed afterwards,
by hand and unchanged, together with this folder; that is why the evidence record carries no
`checkpoint_commit`.

### T5 `/session --export` — green

`Exported 1 session (7 files)`. The zip lands at the checkout root (the export default), not
under `.codeleadsessions\` as the smoke doc said (corrected). Moved into `sessions/` here.

## Differences from the Mac, as expected and as found

- **Expected, confirmed:** a smaller gate (no Mono, no database pins); backslashes in printed
  paths (`docs\codelead\dossier/`) while the survey's own paths stay forward-slash; tsx via
  `npx` on first use.
- **Found:** the survey spawn bug (fixed); the missing local config on a fresh clone (fixed on
  `pilot`); the missing `CODELEAD_CHECKPOINT_COMMITS` in the doc (corrected); faster than the
  Mac's range for T4, with only one sample.

## Seen on Windows, not fixed (for the backlog)

1. **Timeout kill is shallow on Windows.** `runValidationCommands` kills the process group on
   POSIX only; on win32 it kills the direct child, so MSBuild worker nodes can survive a timeout.
   The unit test "kills the whole process group on timeout" also fails on the VM.
2. **Durations use the wall clock.** In the second T3 re-run, "build SpaceBattle" recorded
   **0 ms** while MSBuild itself reported 6.78 s elapsed; in the third, 4,850 ms against
   6.54 s. `durationMs` is `new Date()` arithmetic clamped at zero, and the VM's clock is
   stepped by time sync. Pass/fail is unaffected; recorded durations on a VM are not reliable.
   A monotonic clock (`performance.now()`) would fix it.
3. **Unit tests on Windows:** 5 failing of 2,738 on the VM — path separators asserted as `/`
   (`planBundle`, `validationScope`), `/dev/null/nope` assumed unwritable (it becomes
   `C:\dev\null\nope`), and timeouts on the slower machine.

## Not covered

- The database pins (smoke doc step 5: the Mac's SQL Server container on `10.0.0.157,1433`
  and a real `connections.local.config`).
- The intake asked interactively (the scripted driver took the defaults).

## Contents of this folder

| Path | What |
|---|---|
| `RESULTS.md` | this note |
| `evidence/ev-20260921210016-e3zan4.json` | T4's evidence record |
| `sessions/run2-T1-T5-session-….zip` | the session export of run 2 (T1–T5; every prompt, response and output) |
| `sessions/run1-stopped-session-….zip` | run 1, stopped at the start of T4 once T3 showed the missing file (T2 and T3 baselines) |
| `sessions/T3-rerun-session-….zip` | the three exclusive `/validate` re-runs |
| `dossier/` | T2's bundle (28 files, including the survey's knowledge store and index) |
| `PROJECT-LOG.md` | the project log CodeLead wrote in the checkout |
