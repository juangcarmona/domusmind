---
id: FR-AREAS-CREATE-WORK-FROM-AREA
type: functional-requirement
title: Start new work from an Area
status: draft
derived-from:
- UC-AREAS-CREATE-WORK-FROM-AREA
verification:
- scenario: Choosing New task on an Area owned by Ana opens the task form with that Area and Ana as assignee already filled in
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-TASK
- TERM-ROUTINE
- TERM-PLAN
provenance:
  source: docs/_legacy/00_product/surfaces/areas.md (Inspector, Interaction)
  confidence: medium
  recovered-from: documentation
---

## Requirement

The Area detail MUST offer explicit New task, New routine and New plan actions that open the relevant form pre-filled with the Area; a task MUST be pre-filled with the Owner as assignee and a plan with the Owner as participant. No generic chooser is shown.

## Rationale

Reduces friction and keeps new work linked to its Area (observed: docs/_legacy/00_product/surfaces/areas.md, Inspector and Interaction).
