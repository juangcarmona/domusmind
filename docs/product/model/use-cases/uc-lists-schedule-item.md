---
id: UC-LISTS-SCHEDULE-ITEM
type: use-case
title: "Give an item a due date, reminder or repeat"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT
  - BR-LISTS-ITEM-IS-NOT-A-TASK
  - BR-LISTS-AGENDA-PROJECTION
  - BR-LISTS-ARCHIVED-READ-ONLY
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
  - TERM-ITEM-REPEAT-RULE
  - TERM-LIST-ITEM
  - TERM-AGENDA
provenance:
  source: "openspec/specs/lists/spec.md (Item Temporal Assignment); docs/_legacy/00_product/surfaces/lists.md (Inspector: Time); docs/_legacy/04_contexts/shared-lists-item-model.md (Setting temporal fields); src/backend/DomusMind.Domain/Lists/SharedList.cs (SetItemTemporal); interview: product owner decision Q-0030 (E-0158); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Make a list item time-aware so it shows up in the Agenda when it matters.

## Trigger

A person sets a due date, reminder or repeat in the Time section of the item inspector.

## Preconditions

The item exists.

## Main Flow

1. The person sets one or more of due date, reminder (date and time) and repeat.
2. The product saves the provided fields and leaves the others as they were.
3. The item becomes eligible to appear in the Agenda.

## Alternative Flows

- The item already had temporal fields: the change is an update of its timing rather than a first scheduling (observed: openspec/specs/lists/spec.md, Item Temporal Assignment).
- The person clears one temporal field only (for example the due date): the product removes that field and keeps the others (decided: Q-0030).

## Failure Conditions

- No temporal field is provided: the change is rejected.
- The list is archived: the change is rejected; archived lists are read-only until restored (decided: Q-0031).

## Postconditions

The item carries the timing and appears in the Agenda accordingly; it is still a list item, not a task or plan.
