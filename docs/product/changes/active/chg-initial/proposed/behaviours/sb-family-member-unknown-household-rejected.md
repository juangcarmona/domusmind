---
id: SB-FAMILY-MEMBER-UNKNOWN-HOUSEHOLD-REJECTED
type: structured-behaviour
title: "A person cannot be added to a missing household"
status: draft
illustrates:
  - BR-FAMILY-PERSON-UNIQUE-IN-HOUSEHOLD
when: "A person is submitted for a household that does not exist"
then:
  - "The addition is rejected"
uses-terms:
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/family/spec.md (Member Addition, scenario \"Member addition fails if the household does not exist\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes every person belongs to an existing household.

## Boundaries

None.
