---
id: BR-LISTS-HOUSEHOLD-SCOPED
type: business-rule
title: "Lists belong to the household and are shared by default"
status: draft
applies-to:
  - BC-LISTS
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
  - TERM-TASK
provenance:
  source: "openspec/specs/lists/spec.md (Purpose); docs/_legacy/04_contexts/shared-lists.md (Invariants: SharedList); docs/_legacy/04_contexts/shared-lists-item-model.md (Agenda Scope Placement Rules); docs/_legacy/00_product/surfaces/lists.md (Purpose, Anti-Patterns: Model drift); interview: product owner decision Q-0027 (E-0158); interview: product owner decision Q-0028 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Every list belongs to exactly one household and every item to exactly one list (observed: docs/_legacy/04_contexts/shared-lists.md, Invariants). A list is shared with the whole household by default and can optionally be private to one person (decided: Q-0027; observed: docs/_legacy/00_product/surfaces/lists.md, Purpose, "shared by default, optionally private"); see BR-LISTS-PRIVATE-LIST.

List items carry no person association of their own: no target person and no item-level Area (decided: Q-0028; see CON-LISTS-ITEM-CAPABILITY-BOUNDARY). Items of shared lists project into the household section of the Agenda (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Agenda Scope Placement Rules). Lists must not be made person-centric in the sense of assigning work to people (observed: docs/_legacy/00_product/surfaces/lists.md, Anti-Patterns).

## Rationale

Lists are shared household memory; anchoring work to a person is what a Task with an assignee is for (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Member-scoped temporal list item). A private list is a visibility choice, not an assignment.

## Examples

- A grocery list is visible to every person in the household.
- A dated item on a shared list appears in the household section of the Agenda, not in one person's lane.

## Exceptions

A list made private to one person is not shared with the rest of the household (decided: Q-0027). How the dated items of a private list appear in the Agenda is not settled by the decision or the sources (inferred: gap; see BR-LISTS-PRIVATE-LIST).
