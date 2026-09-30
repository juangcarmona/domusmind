---
id: SB-LISTS-SEQUENTIAL-CAPTURE
type: structured-behaviour
title: "Several items can be captured in a row without interruption"
status: draft
illustrates:
  - UC-LISTS-ADD-ITEM
  - BR-LISTS-NEW-ITEM-UNCHECKED-APPENDED
when: "a person adds several items one after another"
then:
  - "each item is appended in the order it was added"
  - "no modal or interruption is required between items"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Addition; Scenario: Sequential item capture)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Capture must be faster than remembering.

## Boundaries

None.
