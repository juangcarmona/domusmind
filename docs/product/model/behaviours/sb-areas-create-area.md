---
id: SB-AREAS-CREATE-AREA
type: structured-behaviour
title: A new Area starts without an Owner
status: draft
illustrates:
- UC-AREAS-CREATE-AREA
- BR-AREAS-OWNERSHIP-OPTIONAL
given:
- A household exists
when: The household creates an Area with a valid name
then:
- The Area is created in that household
- The Area has no Owner
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Household creates an Area)'
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes that creating an Area needs only a name and that ownership is not assumed.

## Boundaries

Does not assert that an Owner cannot be chosen at creation; the surface allows an optional Owner and Support.
