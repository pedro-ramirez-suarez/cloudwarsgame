# Legacy System Dossier — pedro-ramirez-suarez-fluffy-system

Generated 2026-09-23T00:58:32.797Z · CodeLead Dossier assembler v0

## Coverage summary

**63% tool-driven by section weight** — 6 automated, 7 partial, 2 manual.
Partial sections are machine-measured with a human judgment on top; manual sections are
human work the tool informs but does not perform. This number is stated in every
engagement and is the metric the tooling is held to.

| # | Section | Coverage | Evidence | Findings |
|---|---|---|---|---|
| D1 | System inventory & architecture map | **AUTOMATED** | `project-map.{json,md}` | 13 modules, 181 files, 24 module-level dependencies (Mermaid diagram included). |
| D2 | Contracts & integrations catalogue | **PARTIAL** | `contracts.{json,md}` | 65 public types and 187 public methods across 9 areas; 1 external integration points (config values redacted). Consumer mapping ("who breaks") is manual in v0. |
| D3 | Dependency & vulnerability inventory | **AUTOMATED** | `dependencies.{json,md}` | 39 packages (0 outdated, 0 with advisories); 14 runtime targets classified against an offline EOL snapshot (2026-09-18). |
| D4 | History intelligence | **AUTOMATED** | `git-history.{json,md}` | 43 commits by 3 authors; 25 hot files, 22 co-change pairs, 3 knowledge-concentration rows. |
| D5 | Business rules found in the code | **PARTIAL** | `business-rules.{json,md}, usage.{json}` | 21 rules lifted with file:line evidence (3 state vocabularies, 11 validation limits, 0 enforced guards, 3 thresholds, 4 author notes); 0 comment/code conflict(s). Every rule states WHAT the code does; the WHY is the interview half below. |
| D5b | Tribal knowledge (interviews) | **MANUAL** | `interviews — not performed by the survey` | Why the rules are what they are, undocumented conventions, and the history no commit records. Captured as `expert_testimony` entries during the engagement. This half is human by nature, not by tooling gap. |
| D6 | Risk, testability & pinnability map | **PARTIAL** | `test-census.{json,md}` | 8 areas measured; 3 test desert(s): CloudWars.Game, CloudWars.Engine, CloudWars.Glue. I/O tangling per area feeds a pinnability HINT; the pinnability judgment itself is human. |
| D7 | Modernization candidates & wavefront | **PARTIAL** | `wavefront.{json,md}` | 6 clusters; 5 proposed waves by dependency direction. Proposal automated; the recommendation is human. |
| D8 | Validation strategy | **MANUAL** | `method template` | Per wave: pin current behavior (characterization tests) → transform → prove (parity + gates). Written by hand from the method, informed by D6. |
| D12 | Coding practices & principles | **PARTIAL** | `coding-practices.{json,md}` | 26 pattern(s) that raise migration risk across 5 principle(s): Fail visibly (12); Avoid shared mutable state (10); Single responsibility — file size (2), …. Only patterns that make a transformation more dangerous are reported — no formatting or naming opinions — and vendored third-party code is skipped. |
| D13 | Modernization readiness | **AUTOMATED** | `modernization-readiness.{json,md}` | 9 project(s): 3 legacy-format, 7 without tests; 2 runtime(s) out of support, 1 ending within a year (shipped snapshot 2026-09-18); build floor: 3 legacy-format .NET projects: builds with MSBuild/Visual Studio on Windows (Mono reference assemblies elsewhere); `dotnet build` needs the SDK-style format. |
| D11 | Improvement & modernization opportunities | **PARTIAL** | `opportunities.{json,md}` | 14 opportunities derived from the findings, ranked by consequence: Move 7 projects off an end-of-life runtime; Pin behaviour in 3 untested areas; Resolve the legacy package manifest against a live advisory feed, …. Each carries its evidence, an effort band, and whether it is mechanical or a judgment call. |
| D14 | Structure from the compiled assemblies | **AUTOMATED** | `functional-structure.{json,md}` | 5 of 9 .NET project(s) read from their built assemblies: 55 public types, 41 actions or page handlers, 1 entity sets; roles 1 api-controller, 1 db-context, 1 entity, 4 mvc-controller. Layer 1 (screens and endpoints) and layer 3 (data) of the functional inventory; the rules (layer 2) are narrated per unit in the next step. |
| D9 | Evidence appendix | **AUTOMATED** | `this document + the artifacts it cites` | Every claim above traces to a named artifact file; each section is labeled automated, partial, or manual. |
| D10 | Browsable knowledge base | **PARTIAL** | `..\..\..\Users\pedro\.copilot\repos\copilot-worktrees\cloudwarsgame\pedro-ramirez-suarez-fluffy-system\docs\codelead\dossier\.codelead\knowledge.json` | Machine-derived findings seeded as trust-labeled knowledge entries (below); the PKB instance renders them. Interview-sourced entries are added during the engagement. |

## Knowledge base seeded (D10)

44 findings recorded into `C:\Users\pedro\.copilot\repos\copilot-worktrees\cloudwarsgame\pedro-ramirez-suarez-fluffy-system\docs\codelead\dossier\.codelead\knowledge.json` from the scans above; the store deduplicates by (type, summary), so it holds fewer distinct entries than that —
runtime-EOL risks, test deserts, co-change obligations, integration contracts, and
bus-factor open questions. Every entry carries its source artifact and a trust level;
interview-sourced entries (`expert_testimony`) are added during the engagement.

## How to read this Dossier

1. **D1/D2** answer *what exists and what it talks to*.
2. **D3/D4/D6** answer *where the risk is*: unsupported runtimes, hot and
   knowledge-concentrated files, and areas no test reaches.
3. **D7/D8** answer *what to do first and how it would be proven*.
4. **D5/D9/D10** are the memory: the rules and tribal knowledge, the evidence trail,
   and the browsable knowledge base that outlives the engagement.
