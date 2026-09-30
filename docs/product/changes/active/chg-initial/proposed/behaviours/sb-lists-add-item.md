---
id: SB-LISTS-ADD-ITEM
type: structured-behaviour
title: "Adding an item appends it unchecked"
status: draft
illustrates:
  - UC-LISTS-ADD-ITEM
  - BR-LISTS-NEW-ITEM-UNCHECKED-APPENDED
given:
  - "a list exists"
when: "a person adds an item with a name"
then:
  - "the item is created unchecked at the end of the list"
  - "the item is immediately available in the list"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Addition; Scenario: Household adds an item to a list)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

New items start unchecked and last.

## Boundaries

None.
