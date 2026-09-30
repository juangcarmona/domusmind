---
id: SB-LISTS-REMOVE-ITEM
type: structured-behaviour
title: "Removing an item leaves the others unchanged"
status: draft
illustrates:
  - UC-LISTS-REMOVE-ITEM
given:
  - "a list item exists"
when: "a person removes it"
then:
  - "the item is no longer in the list"
  - "the remaining items are unchanged"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Removal; Scenario: Household removes an item)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Removal is targeted and permanent.

## Boundaries

None.
