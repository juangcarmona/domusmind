---
id: SB-FAMILY-HOUSEHOLD-CREATED
type: structured-behaviour
title: "Household is created from a name"
status: draft
illustrates:
  - UC-FAMILY-CREATE-HOUSEHOLD
given:
  - "A valid household name is provided"
when: "The household is created"
then:
  - "A household exists with a stable identity"
  - "The household has no pets or relationships yet"
uses-terms:
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/family/spec.md (Household Creation, scenario \"Household is created\"); interview: product owner decision Q-0022 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes that a name alone is enough to start a household.

## Boundaries

Does not assert how the creator appears in the roster; the creator is the household's first manager (decided: Q-0022, see BR-FAMILY-CREATOR-IS-FIRST-MANAGER).
