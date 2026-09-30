---
id: TERM-MEMBER-ROLE
type: domain-term
title: "Person Role"
status: draft
defined-in: BC-FAMILY
synonyms:
  - "Member Role"
  - Role
uses-terms:
  - TERM-MEMBER
  - TERM-PET
provenance:
  source: "openspec/specs/family/spec.md (Purpose, Member Addition, Pet Registration); docs/_legacy/04_contexts/family.md (Pet); docs/_legacy/04_contexts/family-member-management.md (Baseline Audit, MemberKind); src/backend/DomusMind.Domain/Family/ValueObjects/MemberRole.cs; interview: product owner decision Q-0021 (E-0158); interview: product owner decision Q-0023 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

The classification of a person within the household. There are three roles: Adult, Child and Pet (decided: Q-0021; observed: src/backend/DomusMind.Domain/Family/ValueObjects/MemberRole.cs). The role decides what the person can do in the rest of the product: the Pet role cannot have access, be assigned tasks or own areas (observed: openspec family spec, Pet Registration).

The specs also named a Caregiver role; it is not part of the product (decided: Q-0021).

## Distinguish From

- Not the Manager designation (TERM-MANAGER), which sits on top of the role.
- Not the planned "Member Kind" (V1.1), which would extend the roles with Guest, External Collaborator, Service Provider and Extended Family (observed: family-member-management.md, MemberKind).

## Usage

Chosen when a person is added and changeable only by a manager, including for the manager's own record; a person who is not a manager cannot change their own role (decided: Q-0023). The Agenda and coordination views include only Adult and Child (observed: docs/_legacy/04_contexts/family.md, Pet). The directory groups people by role (observed: openspec family spec, Household Member Directory).
