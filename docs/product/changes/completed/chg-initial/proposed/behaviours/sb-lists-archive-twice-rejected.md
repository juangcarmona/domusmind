---
id: SB-LISTS-ARCHIVE-TWICE-REJECTED
type: structured-behaviour
title: "Archiving an already archived list is rejected"
status: draft
illustrates:
  - UC-LISTS-ARCHIVE-LIST
  - BR-LISTS-ARCHIVE-STATE
given:
  - "a list is already archived"
when: "a person tries to archive it again"
then:
  - "the archive is rejected"
uses-terms:
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Archive; Scenario: Household attempts to archive an already-archived list)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Archive is only valid from the active state.

## Boundaries

None.
