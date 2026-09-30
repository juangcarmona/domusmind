---
id: SB-LISTS-ARCHIVE
type: structured-behaviour
title: "Archiving a list keeps its items"
status: draft
illustrates:
  - UC-LISTS-ARCHIVE-LIST
  - BR-LISTS-ARCHIVE-STATE
given:
  - "an active list exists"
when: "a person archives it"
then:
  - "the list is no longer among the active lists"
  - "all its items remain intact"
uses-terms:
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Archive; Scenario: Household archives a list)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Archiving is non-destructive.

## Boundaries

None.
