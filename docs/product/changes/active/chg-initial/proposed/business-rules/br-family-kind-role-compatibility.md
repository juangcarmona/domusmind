---
id: BR-FAMILY-KIND-ROLE-COMPATIBILITY
type: business-rule
title: "A person's kind limits their household role"
status: draft
applies-to:
  - UC-FAMILY-ASSIGN-HOUSEHOLD-ROLE
uses-terms:
  - TERM-HOUSEHOLD-ROLE
  - TERM-MEMBER-ROLE
provenance:
  source: "docs/_legacy/04_contexts/family-member-management.md (HouseholdRole, Invariants MemberKind + HouseholdRole); interview: product owner decision Q-0021 (E-0158); interview: product owner decision Q-0022 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Rule

Planned: V1.1. Each kind of person may hold only certain household roles: adults may be Manager, Participant or Observer; children Participant or Observer; guests only Observer; external collaborators Collaborator or Observer; service providers only Service; extended family Observer or Collaborator (observed: family-member-management.md, Constraints on HouseholdRole by MemberKind).

The proposal also lists caregivers; there is no Caregiver role and only adults can be managers (decided: Q-0021, Q-0022), so those entries do not apply.

## Rationale

Keeps authority proportional to how the person relates to the household (inferred).

## Examples

- A guest can only observe.
- A child cannot be made a manager.

## Exceptions

None stated.
