---
id: SB-LISTS-REMOVE-DATED-ITEM
type: structured-behaviour
title: "Removing a dated item takes it out of the Agenda"
status: draft
illustrates:
  - UC-LISTS-REMOVE-ITEM
  - BR-LISTS-AGENDA-PROJECTION
given:
  - "a list item with a due date appears in the Agenda"
when: "a person removes the item"
then:
  - "the item no longer appears in the Agenda"
uses-terms:
  - TERM-PROJECTED-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Removal; Scenario: Removing an item with temporal fields clears its Agenda projection)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

The Agenda never shows items that no longer exist.

## Boundaries

None.
