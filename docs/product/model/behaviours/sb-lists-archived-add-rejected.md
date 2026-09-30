---
id: SB-LISTS-ARCHIVED-ADD-REJECTED
type: structured-behaviour
title: "Adding or checking items on an archived list is rejected"
status: draft
illustrates:
  - BR-LISTS-ARCHIVED-READ-ONLY
  - UC-LISTS-ADD-ITEM
given:
  - "a list has been archived"
when: "a person tries to add an item to it or check one of its items"
then:
  - "the change is rejected"
  - "the list's items stay as they were when it was archived"
uses-terms:
  - TERM-ARCHIVED-LIST
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Addition); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "interview"
---

## Intent

Archived lists are read-only until restored.

## Boundaries

Does not cover list-level changes to an archived list other than restore.
