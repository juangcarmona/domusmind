---
id: SB-FAMILY-HOUSEHOLD-EMPTY-NAME-REJECTED
type: structured-behaviour
title: "Household without a name is rejected"
status: draft
illustrates:
  - BR-FAMILY-HOUSEHOLD-NAME-REQUIRED
when: "Someone tries to create a household with an empty or missing name"
then:
  - "The household is not created"
  - "A validation error is shown"
uses-terms:
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/family/spec.md (Household Creation, scenario \"Household creation fails with an empty name\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes the name is mandatory.

## Boundaries

None.
