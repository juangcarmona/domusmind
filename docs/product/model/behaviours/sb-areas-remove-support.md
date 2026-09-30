---
id: SB-AREAS-REMOVE-SUPPORT
type: structured-behaviour
title: Removing one Support person leaves everyone else in place
status: draft
illustrates:
- UC-AREAS-REMOVE-SUPPORT
given:
- An Area has at least one Support person
when: The household removes one of the Support people
then:
- That person is no longer Support of the Area
- All other Support people remain
- The Owner is unchanged
uses-terms:
- TERM-AREA-SUPPORT
- TERM-AREA-OWNER
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Household removes a secondary owner)'
  confidence: high
  recovered-from: documentation
---

## Intent

Shows that removing Support is a targeted change.

## Boundaries

None.
