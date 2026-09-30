---
id: FR-AREAS-LINKED-WORK
type: functional-requirement
title: Show the work linked to an Area
status: draft
derived-from:
- UC-AREAS-REVIEW-OWNERSHIP
verification:
- scenario: An Area with two open tasks linked shows an open-task count of two on its row and lists both tasks in its detail
- scenario: An Area with a linked shopping list shows no list count on its row and shows the list in its detail
uses-terms:
- TERM-AREA
- TERM-TASK
- TERM-PLAN
- TERM-ROUTINE
- TERM-LIST
provenance:
  source: "docs/_legacy/00_product/surfaces/areas.md (Default View, Inspector, Data, Interaction); interview: product owner decision Q-0020 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

Each Area row MUST show compact counts of linked open tasks, plans and routines when available; lists are not counted on the row. The Area detail MUST list the linked tasks, plans and routines, each openable for editing, and MUST show the Area's linked lists as contextual memory (decided: Q-0020). Owner and Support names MUST lead to that person's Agenda. Counts are cues, not the main content.

## Rationale

Connects accountability to the work it covers without turning Areas into a task board (observed: docs/_legacy/00_product/surfaces/areas.md, Default View, Inspector, Data). The surface names linked lists in its Data section but not in the row definition; lists belong in the Area detail (decided: Q-0020).
