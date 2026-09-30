---
id: FR-FAMILY-ADD-MEMBER
type: functional-requirement
title: "Add a person with a name and role"
status: draft
derived-from:
  - UC-FAMILY-ADD-MEMBER
  - BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE
  - BR-FAMILY-PERSON-UNIQUE-IN-HOUSEHOLD
  - BR-FAMILY-MANAGERS-ADMINISTER
  - BR-FAMILY-MANAGER-MUST-BE-ADULT
verification:
  - scenario-ref: "SB-FAMILY-MEMBER-ADDED"
  - scenario-ref: "SB-FAMILY-DUPLICATE-MEMBER-REJECTED"
  - scenario-ref: "SB-FAMILY-INVALID-ROLE-REJECTED"
  - scenario-ref: "SB-FAMILY-MEMBER-UNKNOWN-HOUSEHOLD-REJECTED"
  - scenario: "A person who is not a manager tries to add a person and is rejected"
uses-terms:
  - TERM-MEMBER
  - TERM-MEMBER-ROLE
provenance:
  source: "openspec/specs/family/spec.md (Member Addition); src/backend/DomusMind.Domain/Family/ValueObjects/MemberRole.cs; interview: product owner decision Q-0021 (E-0158); interview: product owner decision Q-0022 (E-0158); interview: product owner decision Q-0024 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a manager add a person to an existing household with a name and one role: Adult, Child or Pet, optionally with a birth date and notes, and, for an adult, the manager designation (decided: Q-0021, Q-0022, Q-0024). Each person MUST get an identity unique in the household that other areas can refer to. The product MUST reject an unknown role, a duplicate identity, an unknown household (observed: openspec/specs/family/spec.md, Member Addition), and a request from a person who is not a manager (decided: Q-0024).

There is no Caregiver role (decided: Q-0021).

## Rationale

People are who plans, tasks and areas are about.
