---
id: SB-LISTS-CHECK-ITEM
type: structured-behaviour
title: "Checking an item keeps it in the list"
status: draft
illustrates:
  - UC-LISTS-TOGGLE-ITEM
  - BR-LISTS-TOGGLE-PRESERVES-ITEM
given:
  - "an unchecked item exists in a list"
when: "a person toggles it"
then:
  - "the item is checked"
  - "the item remains in the list"
uses-terms:
  - TERM-CHECKED-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Toggle; Scenario: Household checks an item)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Checking is not removal; lists are reusable.

## Boundaries

None.
