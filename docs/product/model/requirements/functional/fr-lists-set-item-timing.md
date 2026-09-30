---
id: FR-LISTS-SET-ITEM-TIMING
type: functional-requirement
title: "Give items a due date, reminder or repeat"
status: draft
derived-from:
  - UC-LISTS-SCHEDULE-ITEM
  - BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT
  - BR-LISTS-ITEM-IS-NOT-A-TASK
verification:
  - scenario-ref: "SB-LISTS-SET-DUE-DATE"
  - scenario-ref: "SB-LISTS-SET-REMINDER-ONLY"
  - scenario-ref: "SB-LISTS-SET-REPEAT-ONLY"
  - scenario: "Clearing only the due date of an item that also has a repeat keeps the repeat, and the item stays in the Agenda through its repeat."
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
  - TERM-ITEM-REPEAT-RULE
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Temporal Assignment); openspec/specs/lists/spec.md (Requirement: Notes: Partial temporal field clearing); src/backend/DomusMind.Domain/Lists/ListItem.cs (SetTemporal); interview: product owner decision Q-0030 (E-0158)"
  confidence: "high"
  recovered-from: "interview"
---

## Requirement

The product MUST let a household set a due date, a reminder (absolute date and time) and/or a repeat rule on an item. At least one MUST be provided; fields not provided MUST stay unchanged. Setting any of them MUST make the item eligible to appear in the Agenda and MUST NOT turn it into a task or create a plan. The first time an item gains timing is a distinct scheduling moment; later changes are updates of its timing.

The product MUST also let a household clear each temporal field of an item on its own (for example remove the due date but keep the repeat), leaving the other temporal fields unchanged (decided: Q-0030; observed: openspec/specs/lists/spec.md, Notes: Partial temporal field clearing). Clearing all three at once is FR-LISTS-CLEAR-ITEM-TIMING. The current code ignores empty values, so a single field cannot be cleared today (observed: src/backend/DomusMind.Domain/Lists/ListItem.cs, SetTemporal); the implementation is incomplete against this requirement.

## Rationale

Time-aware items let the household see list work in the Agenda without a task system.
