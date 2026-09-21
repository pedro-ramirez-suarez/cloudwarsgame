# Business rules found in the code — cloudwarsgame

Generated 2026-09-21T20:43:53.451Z · 68 source files scanned

> Rules are lifted deterministically from the source and are labelled INFERRED: they state what the code does, not why. The 'why' — and any rule that lives only in someone's head — requires interviews (D5 manual half). Where a comment contradicts the code, the code is treated as authoritative.

## Summary

| What | Count |
|---|---|
| State vocabularies (enums) | 3 |
| Validation limits | 11 |
| Enforced guards | 0 |
| Thresholds and constants | 3 |
| Author notes (TODO/HACK/…) | 4 |
| **Comment/code conflicts** | **0** |
| **Rules that disagree with each other** | **0** |

## Rules by kind

### State vocabularies (3)

| Rule | Where |
|---|---|
| Command is one of: NotYourTurn, ChallengePlayer, ChallengeAccepted, StartGame, EndGame, PlayerOnline, PlayerOffLine, GameAction | `CloudWars.Common/Other/Enums.cs:8` |
| GameAction is one of: PlayerReady, UnitMoved, Attack, ShotMade, ShotMissed | `CloudWars.Common/Other/Enums.cs:21` |
| ManageMessageId is one of: ChangePasswordSuccess, SetPasswordSuccess, RemoveLoginSuccess | `CloudWars.Game/Controllers/AccountController.cs:342` |

### Validation limits (11)

| Rule | Where |
|---|---|
| UserName must satisfy Required | `CloudWars.Game/Models/AccountModels.cs:32` |
| OldPassword must satisfy Required | `CloudWars.Game/Models/AccountModels.cs:41` |
| NewPassword must satisfy Required | `CloudWars.Game/Models/AccountModels.cs:46` |
| NewPassword must satisfy StringLength (100, ErrorMessage = "The {0} must be at least {2} characters) | `CloudWars.Game/Models/AccountModels.cs:47` |
| ConfirmPassword must satisfy Compare ("NewPassword", ErrorMessage = "The new password and confirma) | `CloudWars.Game/Models/AccountModels.cs:54` |
| UserName must satisfy Required | `CloudWars.Game/Models/AccountModels.cs:60` |
| Password must satisfy Required | `CloudWars.Game/Models/AccountModels.cs:64` |
| UserName must satisfy Required | `CloudWars.Game/Models/AccountModels.cs:75` |
| Password must satisfy Required | `CloudWars.Game/Models/AccountModels.cs:79` |
| Password must satisfy StringLength (100, ErrorMessage = "The {0} must be at least {2} characters) | `CloudWars.Game/Models/AccountModels.cs:80` |
| ConfirmPassword must satisfy Compare ("Password", ErrorMessage = "The password and confirmation pa) | `CloudWars.Game/Models/AccountModels.cs:87` |

### Thresholds and constants (3)

| Rule | Where |
|---|---|
| difference.TotalSeconds is compared against 30 (>) | `CloudWars.Engine/WorkerRole.cs:135` |
| Length is compared against 50 (>) | `CloudWars.Game/Controllers/PlayerController.cs:39` |
| displayName.Length is compared against 50 (>) | `CloudWars.Game/Controllers/PlayerController.cs:65` |

### Author notes (4)

| Rule | Where |
|---|---|
| NOTE: Turn does NOT switch on a hit (only on a miss). | `CloudWars.Characterization.Db/SpaceBattleGameDbTests.cs:304` |
| TODO: instantiate the proper factory | `CloudWars.Engine/WorkerRole.cs:34` |
| NOTE: For instructions on enabling IIS6 or IIS7 classic mode, | `CloudWars.Game/Global.asax.cs:12` |
| TODO: tell the client that the other player is not online | `CloudWars.Game/Controllers/GameController.cs:213` |

## Component usage — what this system consumes

| Library | Kind | Areas | Imports | References | Verdict |
|---|---|---|---|---|---|
| `CloudWars.Entities` | internal | 7 | 25 | 8 | **used** |
| `CloudWars.Common` | internal | 6 | 22 | 7 | **used** |
| `CloudWars.DataAccess` | internal | 5 | 12 | 7 | **used** |
| `Needletail.DataAccess` | external | 3 | 8 | 3 | **used** |
| `ClourWars.Web` | internal | 1 | 4 | 7 | **used** |
| `CloudWars.SpaceBattle` | internal | 4 | 9 | 1 | **used** |
| `CloudWars.Game` | internal | 1 | 4 | 5 | **used** |
| `Xunit` | external | 2 | 7 | 0 | imported only |
| `Microsoft.WindowsAzure` | external | 1 | 4 | 0 | imported only |
| `Needletail.Mvc` | external | 1 | 4 | 0 | imported only |
| `Impulso.Azure` | external | 1 | 2 | 0 | imported only |
| `Microsoft.SqlServer` | external | 2 | 2 | 0 | imported only |
| `Microsoft.Web` | external | 1 | 2 | 0 | imported only |
| `WebMatrix.WebData` | external | 1 | 2 | 0 | imported only |
| `Microsoft.Security` | external | 1 | 2 | 0 | imported only |
| `CloudWars.Web` | internal | 1 | 1 | 1 | **used** |
| `DotNetOpenAuth.AspNet` | external | 1 | 1 | 0 | imported only |
