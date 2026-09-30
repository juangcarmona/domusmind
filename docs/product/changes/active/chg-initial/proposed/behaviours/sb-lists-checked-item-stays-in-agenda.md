---
id: SB-LISTS-CHECKED-ITEM-STAYS-IN-AGENDA
type: structured-behaviour
title: "A checked item with a due date stays in the Agenda, de-emphasised"
status: draft
illustrates:
  - UC-LISTS-TOGGLE-ITEM
  - UC-LISTS-SEE-ITEMS-IN-AGENDA
  - BR-LISTS-AGENDA-PROJECTION
given:
  - "a list item has a due date"
  - "the item is checked"
when: "a person views the Agenda for that date"
then:
  - "the item still appears"
  - "the item is de-emphasised"
  - "the item leaves the Agenda only once its temporal fields are cleared"
uses-terms:
  - TERM-CHECKED-ITEM
  - TERM-PROJECTED-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Toggle; Scenario: Checked item with a due date remains in Agenda); openspec/specs/lists/spec.md (Requirement: Agenda Projection; Scenario: Checked item continues to project)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Checking does not clear timing; the Agenda keeps showing handled items in a quieter state.

## Boundaries

None.
