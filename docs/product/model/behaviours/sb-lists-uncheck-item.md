---
id: SB-LISTS-UNCHECK-ITEM
type: structured-behaviour
title: "Unchecking an item makes it relevant again"
status: draft
illustrates:
  - UC-LISTS-TOGGLE-ITEM
  - BR-LISTS-TOGGLE-PRESERVES-ITEM
given:
  - "a checked item exists in a list"
when: "a person toggles it"
then:
  - "the item is unchecked"
  - "the item is treated as relevant again"
uses-terms:
  - TERM-CHECKED-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Toggle; Scenario: Household unchecks an item)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Toggle is reversible for the next use of the list.

## Boundaries

None.
