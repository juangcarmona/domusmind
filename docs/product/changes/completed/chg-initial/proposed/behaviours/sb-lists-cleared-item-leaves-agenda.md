---
id: SB-LISTS-CLEARED-ITEM-LEAVES-AGENDA
type: structured-behaviour
title: "An item leaves the Agenda once its timing is cleared but stays in its list"
status: draft
illustrates:
  - UC-LISTS-CLEAR-ITEM-SCHEDULE
  - UC-LISTS-SEE-ITEMS-IN-AGENDA
  - BR-LISTS-AGENDA-PROJECTION
given:
  - "a list item appears in the Agenda"
when: "a person clears all its temporal fields"
then:
  - "the item no longer appears in the Agenda"
  - "the item still exists in its list"
uses-terms:
  - TERM-PROJECTED-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Agenda Projection; Scenario: Item is removed from Agenda after temporal fields are cleared)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Projection follows the item's timing only.

## Boundaries

None.
