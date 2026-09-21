# Modernization dossier bundle

Produced by CodeLead `/modernize --plan` on 2026-09-21 from a read-only survey of this repository. Nothing outside this folder was written.

## Contents

- `DOSSIER.md` and `dossier-index.json`: the survey (sections D1–D13, each labelled automated / partial / manual) with its per-section artifacts.
- `modernization-readiness.{json,md}` (D13): projects, formats and targets, runtime support status, idioms with reasons, tests, build floor.
- `intake.json` and `intake.md`: the client's goals and constraints (defaults; not yet answered); rows marked assumed took a default.
- `options.{json,md}`: the modernization types that fit, with scope, risks, preconditions, consequences, estimates and their basis, and the recommended sequence.
- `knowledge-seeded.json` / `.codelead/`: the survey's knowledge store (D10).

## Writing the client documents

Paste `docs/dossier-writer-brief.md` (from the CodeLead repository) into a Claude session together with the files above, plus the research brief if the engagement uses it. The brief produces the dossier and the technical appendix; every number in them must come from these files. For a client whose code may not leave the machine, run `/modernize --plan --redacted` and send `redacted/` instead (see `docs/data-handling.md` in the CodeLead repository).

## Notes

- No interactive prompt available: the intake took every default; run /modernize --plan --intake from the terminal to answer.
