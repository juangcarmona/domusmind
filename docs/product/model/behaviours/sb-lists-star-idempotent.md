---
id: SB-LISTS-STAR-IDEMPOTENT
type: structured-behaviour
title: "Starring an already starred item is harmless"
status: draft
illustrates:
  - UC-LISTS-SET-ITEM-IMPORTANCE
  - BR-LISTS-IMPORTANCE-BINARY
given:
  - "a list item is already marked important"
when: "a person marks it as important again"
then:
  - "the action succeeds without error"
  - "the item is unchanged"
uses-terms:
  - TERM-ITEM-IMPORTANCE
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Importance; Scenario: Setting importance is idempotent)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Setting importance is idempotent.

## Boundaries

None.
