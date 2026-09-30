---
id: SB-FAMILY-INVALID-ROLE-REJECTED
type: structured-behaviour
title: "A person with an unknown role is rejected"
status: draft
illustrates:
  - BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE
when: "A person is submitted with a role outside Adult, Child and Pet"
then:
  - "The person is not added"
  - "A validation error is shown"
uses-terms:
  - TERM-MEMBER-ROLE
provenance:
  source: "openspec/specs/family/spec.md (Member Addition, scenario \"Member addition fails with an invalid role\"); interview: product owner decision Q-0021 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes the closed set of roles.

## Boundaries

None. There is no Caregiver role (decided: Q-0021).
