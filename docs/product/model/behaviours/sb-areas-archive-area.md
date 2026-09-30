---
id: SB-AREAS-ARCHIVE-AREA
type: structured-behaviour
title: An archived Area leaves the active view but is kept
status: draft
illustrates:
- UC-AREAS-ARCHIVE-AREA
given:
- An Area exists
when: The household archives the Area
then:
- The Area is marked as archived
- The Area is not shown in the default active view
- The Area is shown when the archived filter is applied
uses-terms:
- TERM-AREA
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Household archives an Area)'
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes archiving as retention, not deletion.

## Boundaries

Does not cover reactivation or permanent deletion, which are unspecified.
