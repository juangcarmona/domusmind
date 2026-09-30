---
id: TERM-MEMBER
type: domain-term
title: "Person"
status: draft
defined-in: BC-FAMILY
synonyms:
  - Member
  - "Family Member"
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-MEMBER-ROLE
provenance:
  source: "openspec/specs/family/spec.md (Purpose, Member Addition, Invariants); docs/_legacy/04_contexts/family.md (Member, Ubiquitous Language Notes); docs/_legacy/04_contexts/family-member-management.md (Baseline Audit); src/backend/DomusMind.Domain/Family/FamilyMember.cs; interview: product owner decision Q-0021 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

Someone who belongs to a household and is part of its roster. Each person belongs to exactly one household, has a name, a role and an identity unique within the household, and may optionally have a birth date, notes and profile details. People can be assigned tasks, own areas and take part in plans; pets are recorded as people with the Pet role and carry restrictions (observed: openspec/specs/family/spec.md, Purpose, Member Addition, Invariants).

## Distinguish From

- Not a sign-in account: a person may or may not have access to DomusMind (see TERM-MEMBER-ACCESS) (observed: openspec family spec, NOTE N7).
- Not a Manager by default: managing is an extra designation (TERM-MANAGER).
- Legacy "Dependent" is retired: children are people with the Child role, and there is no Caregiver role (observed: openspec family spec, NOTE N1; decided: Q-0021).
- The legacy documents discourage "user", "profile", "contact" or "participant" as synonyms (observed: docs/_legacy/04_contexts/family.md, Ubiquitous Language Notes).

## Usage

Other areas refer to a person only by identity, as a plan participant, task assignee or area owner; only Household and Members may change who a person is (observed: openspec family spec, Identity Boundary Enforcement).
