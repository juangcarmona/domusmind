---
id: CON-PRODUCT-CURRENT-SCOPE
type: constraint
title: "The current product covers six household areas and excludes the rest"
status: draft
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "docs/_legacy/01_system/system-spec.md (V1 Bounded Contexts, Meal Planning (V2 Extension), Out of Scope for V1); docs/_legacy/01_system/system-overview.md (Core System Model, System Boundaries); docs/_legacy/09_roadmap/roadmap.md (Out of Scope for the Current Phase); decision Q-0001"
  confidence: "medium"
  recovered-from: "documentation"
---

## Constraint

The current product covers the household and its people, areas of responsibility, plans with the Agenda and Phase 1 external calendars, tasks and routines, shared lists, and meal planning with recipes (observed: docs/_legacy/01_system/system-spec.md, V1 Bounded Contexts; docs/_legacy/01_system/system-overview.md, Core System Model; meal planning is current product by decision Q-0001).

Outside the current product (observed: docs/_legacy/01_system/system-spec.md, Out of Scope for V1; docs/_legacy/01_system/system-overview.md, System Boundaries; docs/_legacy/09_roadmap/roadmap.md, Out of Scope for the Current Phase):

- finance, including any full finance platform;
- property management and inventory automation;
- documents as a standalone area;
- pets as a separate operational area (pets exist only within the household's people);
- AI automation and advanced autonomous agents;
- external integrations beyond Phase 1 read-only Outlook import, including bidirectional calendar sync, any write-back to Outlook and a large integration catalogue;
- external identity federation as a requirement, and complex multi-tenant concerns as a primary driver.

## Rationale

The roadmap builds a strong household operating core first; product clarity beats scope growth (observed: docs/_legacy/09_roadmap/roadmap.md, Purpose, Planning Rules 2).

## Consequences

- The excluded areas that the roadmap names as future (administration, documents, property and maintenance, inventory, integrations, intelligence) are modelled only as low-confidence roadmap drafts under CON-PRODUCT-PHASED-ROADMAP (decision Q-0004).
- Conflict resolved: docs/_legacy/01_system/system-spec.md and docs/_legacy/03_domain/context-map.md call Meal Planning a V2 extension; decision Q-0001 makes it current product, so it is in scope here.
- Current product messaging must stay grounded in what exists now (observed: docs/_legacy/09_roadmap/roadmap.md, Planning Rule 4).
