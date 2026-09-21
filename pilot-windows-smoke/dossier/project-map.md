# Architecture map — cloudwarsgame

Generated 2026-09-21T20:43:29.276Z · 150 files indexed · 500 file-level relationships

Profile: {"rootPath":"C:\\src\\cloudwarsgame","projectType":"existing","isGreenfield":false,"detectedLanguages":[{"name":"C#","confidence":0.95,"evidence":["Found 68 .cs files","Found CloudWars.Characterization.Db/CloudWars.Characterization.Db.csproj","Found CloudWars.Characterization/CloudWars.Characterization.csproj","Found CloudWars.Common/CloudWars.Common.csproj","Found CloudWars.sln"]},{"name":"JavaScript","confidence":0.72,"evidence":["Found 40 .js files"]}],"detectedTools":[{"name":".NET","category":"runtime","confidence":0.95,"evidence":["Found CloudWars.Characterization.Db/CloudWars.Characterization.Db.csproj","Found CloudWars.Characterization/CloudWars.Characterization.csproj","Found CloudWars.Common/CloudWars.Common.csproj","Found CloudWars.DataAccess/CloudWars.DataAccess.csproj"]},{"name":".NET test","category":"test","confidence":0.85,"evidence":["Found common .NET test package in .csproj"]}],"importantFiles":[{"path":"CloudWars.Characterization.Db/CloudWars.Characterization.Db.csproj","reason":"C# project file."},{"path":"CloudWars.Characterization/CloudWars.Characterization.csproj","reason":"C# project file."},{"path":"CloudWars.Common/CloudWars.Common.csproj","reason":"C# project file."},{"path":"CloudWars.DataAccess/CloudWars.DataAccess.csproj","reason":"C# project file."},{"path":"CloudWars.Engine/CloudWars.Engine.csproj","reason":"C# project file."},{"path":"CloudWars.Entities/CloudWars.Entities.csproj","reason":"C# project file."},{"path":"CloudWars.Game/CloudWars.Game.csproj","reason":"C# project file."},{"path":"CloudWars.Glue/CloudWars.Glue.csproj","reason":"C# project file."},{"path":"CloudWars.SpaceBattle/CloudWars.SpaceBattle.csproj","reason":"C# project file."},{"path":"CloudWars.sln","reason":"C# solution file."}],"suggestedValidationCommands":["dotnet build","dotnet test"],"summary":"Detected an existing project using C#, JavaScript; tools include .NET, .NET test.","notes":["This appears to be an existing project.","Follow existing project structure, languages, tools, dependencies, and patterns.","Do not introduce new dependencies unless clearly needed.","Use suggested validation commands when appropriate: dotnet build, dotnet test."]}

## Modules

| Module | Path | Role | Files | Summary |
|---|---|---|---|---|
| **CloudWars.Game** | `CloudWars.Game` | source | 84 | CloudWars.Game contains 84 source files in C#, CSS, HTML, JavaScript, Markdown, XML. |
| **CloudWars.Common** | `CloudWars.Common` | source | 11 | CloudWars.Common contains 11 source files in C#, XML. |
| **CloudWars.Entities** | `CloudWars.Entities` | source | 10 | CloudWars.Entities contains 10 source files in C#, XML. |
| **CloudWars.DataAccess** | `CloudWars.DataAccess` | source | 9 | CloudWars.DataAccess contains 9 source files in C#, XML. |
| **CloudWars.Characterization.Db** | `CloudWars.Characterization.Db` | test | 7 | CloudWars.Characterization.Db contains 7 test files in C#, XML. |
| **CloudWars.SpaceBattle** | `CloudWars.SpaceBattle` | source | 7 | CloudWars.SpaceBattle contains 7 source files in C#, XML. |
| **pilot-after** | `pilot-after` | data | 5 | pilot-after contains 5 data files in JSON. |
| **(root)** | `(root)` | mixed | 4 | (root) contains 4 mixed files in C#, Markdown, XML. |
| **pilot-before** | `pilot-before` | data | 4 | pilot-before contains 4 data files in JSON. |
| **CloudWars.Characterization** | `CloudWars.Characterization` | test | 3 | CloudWars.Characterization contains 3 test files in C#, XML. |
| **CloudWars.Engine** | `CloudWars.Engine` | source | 3 | CloudWars.Engine contains 3 source files in C#, XML. |
| **CloudWars.Glue** | `CloudWars.Glue` | source | 3 | CloudWars.Glue contains 3 source files in C#, XML. |

## Module dependency graph

```mermaid
flowchart LR
  CloudWars_Game["CloudWars.Game\n84 files · source"]
  CloudWars_Common["CloudWars.Common\n11 files · source"]
  CloudWars_Entities["CloudWars.Entities\n10 files · source"]
  CloudWars_DataAccess["CloudWars.DataAccess\n9 files · source"]
  CloudWars_Characterization_Db["CloudWars.Characterization.Db\n7 files · test"]
  CloudWars_SpaceBattle["CloudWars.SpaceBattle\n7 files · source"]
  pilot_after["pilot-after\n5 files · data"]
  _root_["(root)\n4 files · mixed"]
  pilot_before["pilot-before\n4 files · data"]
  CloudWars_Characterization["CloudWars.Characterization\n3 files · test"]
  CloudWars_Engine["CloudWars.Engine\n3 files · source"]
  CloudWars_Glue["CloudWars.Glue\n3 files · source"]
  CloudWars_Characterization_Db -->|5| CloudWars_Common
  CloudWars_Characterization_Db -->|6| CloudWars_DataAccess
  CloudWars_Characterization_Db -->|5| CloudWars_Entities
  CloudWars_Characterization_Db -->|16| CloudWars_SpaceBattle
  CloudWars_Characterization -->|5| CloudWars_Common
  CloudWars_Characterization -->|1| CloudWars_Entities
  CloudWars_Characterization -->|4| CloudWars_SpaceBattle
  CloudWars_DataAccess -->|5| CloudWars_Entities
  CloudWars_Engine -->|1| CloudWars_Common
  CloudWars_Engine -->|3| CloudWars_DataAccess
  CloudWars_Entities -->|1| CloudWars_Common
  CloudWars_Game -->|1| CloudWars_Common
  CloudWars_Game -->|3| CloudWars_DataAccess
  CloudWars_Game -->|3| CloudWars_Entities
  CloudWars_Game -->|1| CloudWars_SpaceBattle
  CloudWars_SpaceBattle -->|5| CloudWars_Common
  CloudWars_SpaceBattle -->|4| CloudWars_DataAccess
  CloudWars_SpaceBattle -->|2| CloudWars_Entities
  CloudWars_Engine -->|1| CloudWars_Entities
  _other_ -->|1| CloudWars_Common
  _other_ -->|4| CloudWars_Game
  _other_ -->|1| CloudWars_SpaceBattle
  _other_ -->|1| CloudWars_Entities
  CloudWars_Game -->|2| _other_
```
