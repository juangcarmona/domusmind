---
id: SB-AREAS-REJECT-DUPLICATE-SUPPORT
type: structured-behaviour
title: The same person cannot be added as Support twice
status: draft
illustrates:
- UC-AREAS-ADD-SUPPORT
- BR-AREAS-UNIQUE-SUPPORT
given:
- A person is already Support of an Area
when: The household attempts to add the same person as Support again
then:
- The assignment is rejected
uses-terms:
- TERM-AREA-SUPPORT
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Duplicate secondary owner is rejected)'
  confidence: high
  recovered-from: documentation
---

## Intent

Pins uniqueness of Support people.

## Boundaries

None.
