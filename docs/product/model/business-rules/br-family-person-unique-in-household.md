---
id: BR-FAMILY-PERSON-UNIQUE-IN-HOUSEHOLD
type: business-rule
title: "A person belongs to exactly one household and is unique in it"
status: draft
applies-to:
  - BC-FAMILY
  - UC-FAMILY-ADD-MEMBER
uses-terms:
  - TERM-MEMBER
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/family/spec.md (Invariants, Member Addition); docs/_legacy/04_contexts/family.md (Identity and Membership); src/backend/DomusMind.Domain/Family/Family.cs (AddMember duplicate check)"
  confidence: high
  recovered-from: documentation
---

## Rule

A person belongs to exactly one household, and each person's identity is unique within it. A person cannot be added to a household that does not exist (observed: openspec/specs/family/spec.md, Invariants, Member Addition). The household's own identity is stable and never changes (observed: Invariants).

## Rationale

Other areas refer to people and households by identity only, so identities must be unambiguous and permanent (observed: openspec family spec, Identity Boundary Enforcement).

## Examples

- Adding a second entry with the same identity as an existing person is rejected (observed: scenario "Member addition fails with a duplicate MemberId").
- Adding a person to an unknown household is rejected (observed: scenario "Member addition fails if the household does not exist").

## Exceptions

Pets share the same identity space as people today (observed: openspec family spec, Invariants).
