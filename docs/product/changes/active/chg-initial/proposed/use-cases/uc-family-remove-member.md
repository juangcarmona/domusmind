---
id: UC-FAMILY-REMOVE-MEMBER
type: use-case
title: "Remove a person or pet from the household"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE
  - BR-FAMILY-MANAGERS-ADMINISTER
uses-terms:
  - TERM-MEMBER
  - TERM-PET
provenance:
  source: "openspec/specs/family/spec.md (Member Removal, Pet Registration, NOTE N3); docs/_legacy/04_contexts/family.md (Commands: RemoveMember, RemovePet); docs/_legacy/04_contexts/family-member-management.md (Group B remove-member); docs/_legacy/01_system/system-spec.md (Deferred to V1.1); interview: product owner decision Q-0024 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Goal

Planned: V1.1. Someone who no longer belongs to the household leaves the roster (observed: openspec/specs/family/spec.md, Member Removal).

## Trigger

A person moves out or a pet is no longer part of the household.

## Preconditions

The person or pet is on the roster and the actor is a manager; only managers remove people and pets (decided: Q-0024).

## Main Flow

1. The actor chooses to remove the person or pet.
2. DomusMind removes them from the roster.

## Alternative Flows

None known.

## Failure Conditions

Not specified. Open tasks and plan participation must be validated before removal, which is why this is deferred (observed: docs/_legacy/01_system/system-spec.md, Deferred to V1.1). Whether a manager can remove themselves, or the last manager, is not specified (observed: openspec family spec, NOTE N3).

## Postconditions

The person no longer appears in the roster and cannot take part in new relationships. Other areas handle their own references; removal does not cascade (observed: openspec family spec). The legacy V1.1 proposal instead turns removal into deactivation then archiving (observed: family-member-management.md, Group B).
