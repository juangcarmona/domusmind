---
id: SB-LISTS-CLEAR-QUANTITY
type: structured-behaviour
title: "Clearing an item's quantity"
status: draft
illustrates:
  - UC-LISTS-UPDATE-ITEM
given:
  - "a list item has a quantity"
when: "a person explicitly clears the quantity"
then:
  - "the item no longer has a quantity"
  - "its name and other fields are unchanged"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Update; Scenario: Household clears an item's quantity)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Quantity can be removed as well as set.

## Boundaries

None.
