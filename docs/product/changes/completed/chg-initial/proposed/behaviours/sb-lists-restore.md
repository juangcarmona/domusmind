---
id: SB-LISTS-RESTORE
type: structured-behaviour
title: "Restoring a list brings it back unchanged"
status: draft
illustrates:
  - UC-LISTS-RESTORE-LIST
  - BR-LISTS-ARCHIVE-STATE
given:
  - "an archived list exists"
when: "a person restores it"
then:
  - "the list is again among the active lists"
  - "all its items are in the same state as when it was archived"
uses-terms:
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Restore; Scenario: Household restores an archived list)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Restore is the exact inverse of archive.

## Boundaries

None.
