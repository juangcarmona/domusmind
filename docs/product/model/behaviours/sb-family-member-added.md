---
id: SB-FAMILY-MEMBER-ADDED
type: structured-behaviour
title: "A person is added to the household"
status: draft
illustrates:
  - UC-FAMILY-ADD-MEMBER
given:
  - "A household exists"
when: "A person is added with a valid name and role"
then:
  - "The person is on the household roster with an identity unique in the household"
uses-terms:
  - TERM-MEMBER
  - TERM-MEMBER-ROLE
provenance:
  source: "openspec/specs/family/spec.md (Member Addition, scenario \"A member is added to the household\"); interview: product owner decision Q-0024 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes the basic way people join a household.

## Boundaries

Does not show the requester; only managers add people (decided: Q-0024).
