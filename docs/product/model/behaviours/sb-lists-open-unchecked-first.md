---
id: SB-LISTS-OPEN-UNCHECKED-FIRST
type: structured-behaviour
title: "Opening a list shows unchecked items before checked ones"
status: draft
illustrates:
  - UC-LISTS-OPEN-LIST
given:
  - "a list has a mix of checked and unchecked items"
when: "a person opens the list"
then:
  - "all items are shown"
  - "unchecked items appear before checked items"
uses-terms:
  - TERM-LIST-ITEM
  - TERM-CHECKED-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Detail; Scenario: Household opens a list)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Remaining work comes first while handled items stay accessible.

## Boundaries

None.
