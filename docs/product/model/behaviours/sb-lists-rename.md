---
id: SB-LISTS-RENAME
type: structured-behaviour
title: "Renaming a list changes only its name"
status: draft
illustrates:
  - UC-LISTS-UPDATE-LIST
given:
  - "a list exists"
when: "a person gives the list a new name"
then:
  - "the list has the new name"
  - "no other detail of the list changes"
uses-terms:
  - TERM-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Update; Scenario: Household renames a list)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Updates are partial: untouched fields stay as they were.

## Boundaries

None.
