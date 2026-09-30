---
id: SB-AREAS-RENAME-AREA
type: structured-behaviour
title: Renaming an Area
status: draft
illustrates:
- UC-AREAS-RENAME-AREA
given:
- An Area exists
when: The household gives the Area a new name
then:
- The Area carries the new name
uses-terms:
- TERM-AREA
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Household renames an Area)'
  confidence: high
  recovered-from: documentation
---

## Intent

Shows renaming as a simple identity change.

## Boundaries

Does not assert anything about ownership, which is unaffected.
