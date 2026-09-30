---
id: SB-LISTS-RENAME-ITEM
type: structured-behaviour
title: "Renaming an item leaves its other capabilities unchanged"
status: draft
illustrates:
  - UC-LISTS-UPDATE-ITEM
given:
  - "a list item exists"
when: "a person gives the item a new name"
then:
  - "the item has the new name"
  - "its checked state, importance and temporal fields are unchanged"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Update; Scenario: Household updates an item's name)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Base-field edits are isolated from other capabilities.

## Boundaries

None.
