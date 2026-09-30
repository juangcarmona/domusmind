---
id: CON-PRODUCT-SEPARATE-AXES
type: constraint
title: "Time, execution, ownership and capture stay separate"
status: draft
applies-to:
  - "BC-CALENDAR"
  - "BC-TASKS"
  - "BC-RESPONSIBILITIES"
  - "BC-LISTS"
  - "BC-MEAL-PLANNING"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-TASK"
  - "TERM-AREA"
  - "TERM-LIST"
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/web-app/spec.md (Surface Axis Separation); docs/_legacy/00_product/experience.md (Surface Roles: Lists); docs/_legacy/01_system/system-overview.md (Agenda as Unified Temporal Surface, Shared Temporal Vocabulary, Context Collaboration); docs/_legacy/01_system/system-spec.md (V1 Bounded Contexts); docs/_legacy/03_domain/context-map.md (Context Execution Boundary, Context Boundaries)"
  confidence: "high"
  recovered-from: "documentation"
---

## Constraint

The product keeps its axes strictly separate, each owned by one part of the product (observed: openspec/specs/web-app/spec.md, Surface Axis Separation; docs/_legacy/00_product/experience.md, Lists "four axes"; docs/_legacy/03_domain/context-map.md, Context Execution Boundary):

| Axis | Owned by | Household surface |
|---|---|---|
| Time | Plans; the Agenda is the unified read surface | Agenda |
| Structured execution | Tasks and routines | shown in Agenda and Areas |
| Ownership | Areas | Areas |
| Capture and flexible execution | Lists | Lists |
| Configuration | people and household preferences, personal integrations | Settings |

No surface owns another surface's items and no semantic collapse is permitted: the Agenda reads from every source but writes to none of them; showing something in the Agenda never transfers its ownership (observed: openspec/specs/web-app/spec.md, Surface Axis Separation; docs/_legacy/01_system/system-overview.md, Agenda as Unified Temporal Surface "Write model is divided. Read model is unified").

## Rationale

A household needs to see plans and work together without the product becoming a muddle of half-tasks and half-events; keeping axes distinct is what makes the unified picture trustworthy (inferred from docs/_legacy/00_product/strategy.md, Differentiators; docs/_legacy/09_roadmap/roadmap.md, V2 Rule "must not blur the boundaries between time, execution, responsibility, list-based coordination").

## Consequences

- A list item with a due date is not a plan and not a task; a routine is not a task; a meal in the Agenda is not a plan (observed: docs/_legacy/03_domain/context-map.md, Context Boundaries; enforced in the areas by BR-LISTS-ITEM-IS-NOT-A-TASK, BR-TASKS-DISTINCT-EXECUTION-MODELS, BR-MEALS-AGENDA-PROJECTION).
- Edits to an item happen on its owning surface; the Agenda offers "Open in ..." instead (observed: openspec/specs/web-app/spec.md, Agenda Projected List Items; BR-AGENDA-PROJECTION-READ-ONLY).
- Shared time vocabulary (due date, reminder, repeat) is reused across areas without sharing records (observed: docs/_legacy/01_system/system-overview.md, Shared Temporal Vocabulary).
- Future domains (V2) must respect the same separation (observed: docs/_legacy/09_roadmap/roadmap.md, V2 Rule).
