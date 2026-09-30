---
id: SB-AREAS-ADD-SUPPORT
type: structured-behaviour
title: Adding Support leaves the Owner unchanged
status: draft
illustrates:
- UC-AREAS-ADD-SUPPORT
given:
- An Area exists
- A person of the household is not yet Support of that Area
when: The household adds that person as Support
then:
- The person is Support of the Area
- The Owner is unchanged
uses-terms:
- TERM-AREA-SUPPORT
- TERM-AREA-OWNER
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Household adds a secondary owner to an Area)'
  confidence: high
  recovered-from: documentation
---

## Intent

Shows Support as additional coverage, never a replacement for the Owner.

## Boundaries

None.
