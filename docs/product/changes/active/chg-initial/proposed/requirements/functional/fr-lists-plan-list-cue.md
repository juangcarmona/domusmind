---
id: FR-LISTS-PLAN-LIST-CUE
type: functional-requirement
title: "Show a compact list cue on plans linked to a list"
status: draft
derived-from:
  - UC-LISTS-SEE-ITEMS-IN-AGENDA
  - UC-AGENDA-VIEW-HOUSEHOLD-AGENDA
  - UC-AGENDA-VIEW-MEMBER-AGENDA
  - BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT
verification:
  - scenario: "A plan linked to a list with five unchecked items shows in the Agenda with the list name and the count 5; selecting the cue opens the list; the list's undated items do not appear in the Agenda."
  - scenario: "The \"Beach trip\" plan has a related packing list with 4 unchecked items; the plan shows \"Packing - 4\" and selecting the cue opens that list in Lists (observed: agenda.md, Lists in Agenda)."
uses-terms:
  - TERM-PLAN
  - TERM-LIST
  - TERM-AGENDA
provenance:
  source: "openspec/specs/web-app/spec.md (Agenda Plan-List Reference Cue); docs/_legacy/00_product/surfaces/lists.md (Relationship with Agenda); docs/_legacy/04_contexts/shared-lists-item-model.md (Plan-linked temporal list item); docs/_legacy/00_product/surfaces/agenda.md (Lists in Agenda)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

When a plan shown in the Agenda has a linked list, the product MUST show a compact cue with the list name and its unchecked count that navigates to the list in Lists, and MUST NOT expand the list's items inline as plan content or convert them to tasks (observed: openspec/specs/web-app/spec.md, Agenda Plan-List Reference Cue, "SHALL surface"; docs/_legacy/00_product/surfaces/lists.md). The legacy agenda surface phrased the cue as optional ("MAY"); the web-app spec makes it mandatory and prevails (observed: agenda.md vs web-app spec).

## Rationale

People preparing for a plan need to reach its list without the Agenda turning into a task batch.
