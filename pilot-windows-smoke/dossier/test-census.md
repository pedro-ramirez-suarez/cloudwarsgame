# Test census & I/O tangling — cloudwarsgame

Generated 2026-09-21T20:43:48.615Z · 83 files scanned · 7 test files

**Test deserts (no reaching tests):** `CloudWars.Game`, `CloudWars.Engine`, `CloudWars.Glue`

Pinnability hint is evidence for a human judgment, not the judgment: it reflects I/O hits per source file (DB/network/filesystem/time/globals).

| Area | Source files | Test files | Reaching tests | Desert | I/O hits (db/net/fs/time/glob) | Pinnability hint |
|---|---|---|---|---|---|---|
| `CloudWars.Game` | 38 | 0 | 0 | **YES** | 1/11/0/44/5 | pin at seams |
| `CloudWars.Engine` | 2 | 0 | 0 | **YES** | 0/0/0/2/0 | likely pinnable |
| `CloudWars.Glue` | 2 | 0 | 0 | **YES** | 0/0/0/0/0 | likely pinnable |
| `CloudWars.DataAccess` | 8 | 0 | 5 | no | 2/0/1/0/0 | likely pinnable |
| `CloudWars.Characterization.Db` | 1 | 5 | 5 | no | 0/0/0/0/1 | likely pinnable |
| `CloudWars.Common` | 10 | 0 | 4 | no | 0/0/0/0/0 | likely pinnable |
| `CloudWars.SpaceBattle` | 6 | 0 | 7 | no | 0/0/0/0/0 | likely pinnable |
| `CloudWars.Entities` | 9 | 0 | 6 | no | 0/0/0/0/0 | likely pinnable |
