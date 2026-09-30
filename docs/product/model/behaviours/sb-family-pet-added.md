---
id: SB-FAMILY-PET-ADDED
type: structured-behaviour
title: "A pet is added to the household"
status: draft
illustrates:
  - UC-FAMILY-REGISTER-PET
given:
  - "A household exists"
when: "A pet is added with a valid name and the Pet role"
then:
  - "The pet is part of the household with its own unique identity"
uses-terms:
  - TERM-PET
provenance:
  source: "openspec/specs/family/spec.md (Pet Registration, scenario \"A pet is added to the household\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes pets are recorded as part of the household.

## Boundaries

None.
