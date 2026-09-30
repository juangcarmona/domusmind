---
id: SB-LISTS-DUE-TODAY-IN-AGENDA
type: structured-behaviour
title: "An item due today appears in today's Agenda with its list origin"
status: draft
illustrates:
  - UC-LISTS-SEE-ITEMS-IN-AGENDA
  - BR-LISTS-AGENDA-PROJECTION
given:
  - "a list item is due today"
when: "a person loads today's Agenda"
then:
  - "the item appears as a list item entry with a list-origin cue"
  - "it is visually distinct from tasks and plans"
uses-terms:
  - TERM-PROJECTED-LIST-ITEM
  - TERM-TASK
  - TERM-PLAN
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Agenda Projection; Scenario: Item with due date appears in Agenda on that date)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

List items are a first-class, distinguishable Agenda source.

## Boundaries

None.
