---
id: SB-LISTS-REORDER
type: structured-behaviour
title: "Reordering changes only positions"
status: draft
illustrates:
  - UC-LISTS-REORDER-ITEMS
  - BR-LISTS-REORDER-FULL-SET
given:
  - "a list has several items"
when: "a person provides a complete new order for those items"
then:
  - "each item takes its new position"
  - "no item field other than its position changes"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Reorder; Scenario: Household reorders items)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Order is presentation, not meaning.

## Boundaries

None.
