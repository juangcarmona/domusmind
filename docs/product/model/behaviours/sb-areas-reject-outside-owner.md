---
id: SB-AREAS-REJECT-OUTSIDE-OWNER
type: structured-behaviour
title: A person outside the household cannot become Owner
status: draft
illustrates:
- UC-AREAS-ASSIGN-OWNER
- BR-AREAS-SAME-HOUSEHOLD-PEOPLE
given:
- A person does not belong to the household
when: The household attempts to assign that person as Owner of one of its Areas
then:
- The assignment is rejected
uses-terms:
- TERM-AREA-OWNER
- TERM-HOUSEHOLD
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Non-family member cannot be assigned as owner)'
  confidence: high
  recovered-from: documentation
---

## Intent

Keeps accountability inside the household.

## Boundaries

None.
