# ADR 0001: Adopt Architecture Decision Records

| Field | Value |
|-------|--------|
| **Status** | Accepted |
| **Date** | 2026-04-03 |

## Context

The project needs a **lightweight, versioned log** of significant architectural choices so future contributors understand **why** the system is shaped as it is—not only **what** the code does. Informal knowledge in chat or tribal memory does not survive handovers.

We follow the practice described in Martin Fowler’s note on [Architecture Decision Records](https://martinfowler.com/bliki/ArchitectureDecisionRecord.html): short documents, one decision per file, stored with the source; **Accepted** records are not rewritten—changes use **Superseded** plus a new ADR.

## Decision

1. **Location:** ADRs live under **`docs/adr/`** in this repository as Markdown (`.md`).
2. **Naming:** Each ADR is **`NNNN-short-title-in-kebab-case.md`** with a **monotonic** serial (`0001`, `0002`, …).
3. **Content:** One decision per file: **Context**, **Decision**, **Consequences**; **Alternatives considered** when helpful. Prefer essentials first ([Fowler](https://martinfowler.com/bliki/ArchitectureDecisionRecord.html)).
4. **Status:** **Proposed** while under discussion; **Accepted** when adopted (immutable); **Superseded** when replaced—with a link to the superseding ADR.
5. **Automation:** Cursor rule **architecture-decision-records** (`.cursor/rules/`) reminds contributors when to add ADRs and how to handle status.

## Consequences

- **Positive:** Clear audit trail; easier onboarding; supersession preserves history.
- **Negative:** Small ongoing cost to write and number ADRs.

## Alternatives considered

- **Wiki only:** Rejected—review and versioning are weaker than `docs/adr/` in git.
- **Single long architecture document:** Rejected—no clean per-decision history or supersession.
