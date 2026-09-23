# Functional inventory — structure — pedro-ramirez-suarez-fluffy-system

Generated 2026-09-23. Read from the built assemblies of 5 of 9 .NET projects: 55 public types, 41 actions or handlers, 1 entity sets. Roles: 1 API controllers, 1 EF contexts, 1 entities, 4 MVC controllers.

### Not read

| Project | Why |
|---|---|
| CloudWars.Characterization.Db | no built assembly under CloudWars.Characterization.Db/bin (build the project first; the survey reads compiled metadata, not source) |
| CloudWars.Characterization | no built assembly under CloudWars.Characterization/bin (build the project first; the survey reads compiled metadata, not source) |
| CloudWars.Engine | no built assembly under CloudWars.Engine/bin (build the project first; the survey reads compiled metadata, not source) |
| CloudWars.Glue | no built assembly under CloudWars.Glue/bin (build the project first; the survey reads compiled metadata, not source) |

### Layer 1 — screens and endpoints

| Project | Type | Role | Action | Verbs | Parameters | Returns |
|---|---|---|---|---|---|---|
| CloudWars.Game | AccountController | mvc-controller | Disassociate | POST | string provider, string providerUserId | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | ExternalLogin | POST | string provider, string returnUrl | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | ExternalLoginCallback | - | string returnUrl | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | ExternalLoginConfirmation | POST | RegisterExternalLoginModel model, string returnUrl | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | ExternalLoginFailure | - | - | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | ExternalLoginsList | - | string returnUrl | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | LogOff | GET | - | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | Login | - | string returnUrl | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | Login | POST | LoginModel model, string returnUrl | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | Manage | - | Nullable<ManageMessageId> message | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | Manage | POST | LocalPasswordModel model | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | Register | - | - | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | Register | POST | RegisterModel model | ActionResult |
| CloudWars.Game | AccountController | mvc-controller | RemoveExternalLogins | - | - | ActionResult |
| CloudWars.Game | HomeController | mvc-controller | About | - | - | ActionResult |
| CloudWars.Game | HomeController | mvc-controller | Bitacora | - | - | ActionResult |
| CloudWars.Game | HomeController | mvc-controller | ComoJugar | - | - | ActionResult |
| CloudWars.Game | HomeController | mvc-controller | Index | - | - | ActionResult |
| CloudWars.Game | HomeController | mvc-controller | Inicio | - | - | ActionResult |
| CloudWars.Game | HomeController | mvc-controller | Tecnologia | - | - | ActionResult |
| CloudWars.Game | GameController | mvc-controller | AcceptChallenge | POST | Guid challengeId | TwoWayResult |
| CloudWars.Game | GameController | mvc-controller | ChallengePlayer | POST | Guid playerId | TwoWayResult |
| CloudWars.Game | GameController | mvc-controller | GetAllPlayers | - | - | ActionResult |
| CloudWars.Game | GameController | mvc-controller | GetNearByPlayers | - | - | ActionResult |
| CloudWars.Game | GameController | mvc-controller | Initialize | POST | string liveId, string latitude, string longitude | void |
| CloudWars.Game | GameController | mvc-controller | InvitePlayer | POST | Guid matchId, string clientId | TwoWayResult |
| CloudWars.Game | GameController | mvc-controller | MyChallenges | POST | - | ActionResult |
| CloudWars.Game | GameController | mvc-controller | MyMatches | POST | - | ActionResult |
| CloudWars.Game | GameController | mvc-controller | OnlinePlayers | - | - | ActionResult |
| CloudWars.Game | GameController | mvc-controller | Play | - | Guid matchId | ActionResult |
| CloudWars.Game | GameController | mvc-controller | PlayMatch | POST | Guid matchId | TwoWayResult |
| CloudWars.Game | GameController | mvc-controller | PlayerAttack | POST | Guid matchId, Guid playerId, int row, int col | void |
| CloudWars.Game | GameController | mvc-controller | PlayerChat | POST | string message, string clientId | TwoWayResult |
| CloudWars.Game | GameController | mvc-controller | PlayerMoveTo | POST | Guid matchId, string unitId, int row, int col | void |
| CloudWars.Game | GameController | mvc-controller | PlayerReady | POST | Guid matchId, Guid playerId | void |
| CloudWars.Game | GameController | mvc-controller | RejectChallenge | POST | Guid challengeId | TwoWayResult |
| CloudWars.Game | PlayerController | mvc-controller | CreateProfile | - | - | ActionResult |
| CloudWars.Game | PlayerController | mvc-controller | CreateProfile | POST | Guid id, string displayName, string avatar | ActionResult |
| CloudWars.Game | PlayerController | mvc-controller | EditProfile | - | - | ActionResult |
| CloudWars.Game | PlayerController | mvc-controller | EditProfile | POST | Guid id, string displayName, string avatar | ActionResult |
| CloudWars.Game | PlayerController | mvc-controller | Index | - | - | ActionResult |

### Layer 3 — data

| Project | Context | Entity sets |
|---|---|---|
| CloudWars.Game | UsersContext | UserProfile (UserProfiles) |

### Public surface per project

| Project | Assembly | Types | Roles | Members |
|---|---|---|---|---|
| CloudWars.Common | CloudWars.Common/bin/Debug/net8.0/CloudWars.Common.dll | 10 | - | 74 |
| CloudWars.DataAccess | CloudWars.DataAccess/bin/Debug/net8.0/CloudWars.DataAccess.dll | 4 | - | 78 |
| CloudWars.Entities | CloudWars.Entities/bin/Debug/net8.0/CloudWars.Entities.dll | 8 | - | 55 |
| CloudWars.Game | CloudWars.Game/bin/CloudWars.Game.dll | 28 | 4 MVC controllers, 1 API controllers, 1 entities, 1 EF contexts | 150 |
| CloudWars.SpaceBattle | CloudWars.SpaceBattle/bin/Debug/net8.0/CloudWars.SpaceBattle.dll | 5 | - | 55 |

### Contracts (interfaces and service contracts)

- CloudWars.Common: `IClientFeedback` — 7 members: ChallengeAccepted, ChallengePlayer, PlayerLost, PlayerWon, ShotMade, ShotMissed, …
- CloudWars.Common: `IFactory` — 4 members: GetClientFeedback, GetGame, GetGame, GetPlayerPresence
- CloudWars.Common: `IGame` — 18 members: AcceptChallenge, ChallengePlayer, CreateMatch, Initialize, MatchFinished, PauseMatch, …
- CloudWars.Common: `IPlayerPresence` — 6 members: GetClientId, GetPlayerId, GetPlayerNameByClientId, IsPlayingMatch, PlayerDisconnected, PlayerIsOnLine
- CloudWars.Common: `IGameUnit` — 10 members: MoveTo, TakeDamage, …
- CloudWars.DataAccess: `ICloudWarsData` — 20 members: AcceptChallenge, AddNotification, ChallengePlayer, CreateMatch, DeleteMatch, GetChallenge, …

### Problems

- CloudWars.Game.MvcApplication: TypeLoadException: Could not find type 'System.Web.HttpApplication' in assembly 'C:\Program Files\dotnet\shared\Microsoft.NETCore.App\10.0.12\System.Web.dll'.

