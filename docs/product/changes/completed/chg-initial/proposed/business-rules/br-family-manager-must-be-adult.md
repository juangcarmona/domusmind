---
id: BR-FAMILY-MANAGER-MUST-BE-ADULT
type: business-rule
title: "Only adults can be managers"
status: draft
applies-to:
  - UC-FAMILY-ADD-MEMBER
  - UC-FAMILY-UPDATE-MEMBER-DETAILS
uses-terms:
  - TERM-MANAGER
  - TERM-MEMBER-ROLE
provenance:
  source: "src/backend/DomusMind.Domain/Family/Family.cs (AddMember, UpdateMember); openspec/specs/family/spec.md (NOTE N4); docs/_legacy/04_contexts/family-member-management.md (Invariants MemberKind + HouseholdRole); interview: product owner decision Q-0022 (E-0158); interview: product owner decision Q-0021 (E-0158)"
  confidence: high
  recovered-from: interview
---

## Rule

A person can be designated manager only when their role is Adult. The manager designation is set when an adult is added or when an adult's details are edited (decided: Q-0022; observed: src/backend/DomusMind.Domain/Family/Family.cs, AddMember and UpdateMember).

The specs left manager designation unspecified (observed: openspec/specs/family/spec.md, NOTE N4). The legacy V1.1 proposal would also have allowed caregivers; there is no Caregiver role and only adults qualify (decided: Q-0021, Q-0022).

## Rationale

Household authority should sit with a responsible adult (inferred).

## Examples

- Adding an adult and marking them as manager: allowed.
- Editing an adult's details and making them a manager: allowed.
- Making a child a manager: rejected.

## Exceptions

None.
