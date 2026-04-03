# Plans

Forward-looking work—**spikes**, **features**, **refactoring** notes, and archived initiatives—lives under this tree.

## Folder layout

| Folder | Use |
|--------|-----|
| **`features/`** | Planned or in-progress product features (multi-step, may link to issues). |
| **`spikes/`** | Time-boxed investigations, prototypes, technical experiments. |
| **`refactoring/`** | Cross-cutting refactors, migrations between patterns, cleanup campaigns. |
| **`archive/`** | Completed or abandoned plans kept for history. |

Each initiative is a **directory** with a **`README.md`** at its root.

## README front matter

Use **YAML front matter** at the top of each plan’s `README.md` for quick scanning:

```yaml
---
status: proposed   # e.g. proposed | active | done | cancelled
phase: discovery   # e.g. discovery | implementation | rollout
updated: 2026-04-03
---
```

Keep **`updated`** current when the plan changes. **status** and **phase** are free-form but should stay consistent across plans in this repo.

## Plans vs ADRs

- **`docs/plans/`** — execution, exploration, sequencing, checklists.
- **`docs/adr/`** — **durable decisions** once the team commits (see [ADR 0001](../adr/0001-adopt-architecture-decision-records.md)).

When a spike concludes with a lasting choice, capture it as a new ADR and link from the plan.
