---
id: CON-PRODUCT-HOUSEHOLD-ASSETS-BOUNDARIES
type: constraint
title: Household Assets reuses existing domain ownership
status: draft
applies-to:
  - BC-HOUSEHOLD-ASSETS
  - BC-FAMILY
  - BC-RESPONSIBILITIES
  - BC-CALENDAR
  - BC-TASKS
  - BC-LISTS
uses-terms:
  - TERM-ASSET
  - TERM-ASSET-OBLIGATION
---
## Constraint

Household Assets owns asset identity, lifecycle, obligations and associated records. Household owns membership and the authorization boundary; Areas owns responsibility structures; Agenda remains the unified temporal read surface; Calendar owns Plans; Tasks owns Tasks/Routines; Lists owns list items. Cross-context references, projections and navigational links do not transfer ownership, create records automatically or silently synchronize lifecycle. Meal Planning has no initial integration.

## Rationale

Household asset management must reduce mental load while preserving the product's explicit context boundaries and established separation of time, execution, responsibility and shared memory.

## Consequences

- Agenda projection is read-oriented and links back to the obligation.
- Any task, appointment or checklist is created explicitly through its owning context.
- Document references retain source authorization; no generic document store is presumed.
- AI assistance and MCP exposure are future consumers, not dependencies or alternate sources of truth.
