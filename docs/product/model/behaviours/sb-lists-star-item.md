---
id: SB-LISTS-STAR-ITEM
type: structured-behaviour
title: "Starring an item"
status: draft
illustrates:
  - UC-LISTS-SET-ITEM-IMPORTANCE
  - BR-LISTS-IMPORTANCE-BINARY
given:
  - "a list item is not marked important"
when: "a person marks it as important"
then:
  - "the item is starred"
  - "no other field of the item changes"
uses-terms:
  - TERM-ITEM-IMPORTANCE
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Importance; Scenario: Household marks an item as important)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Importance is an isolated flag.

## Boundaries

None.
