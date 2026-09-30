---
id: UC-FAMILY-MANAGE-RELATIONSHIPS
type: use-case
title: "Record relationships between people"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-RELATIONSHIP-INTEGRITY
  - BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE
  - BR-FAMILY-MANAGERS-ADMINISTER
uses-terms:
  - TERM-RELATIONSHIP
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Relationship Assignment); docs/_legacy/04_contexts/family.md (Relationship, Commands: AssignRelationship, RemoveRelationship); docs/_legacy/01_system/system-spec.md (Deferred to V1.1); interview: product owner decision Q-0021 (E-0158); interview: product owner decision Q-0024 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Goal

Planned: V1.1. The household records who is whose parent, spouse or sibling, and who looks after whom (observed: openspec/specs/family/spec.md, Relationship Assignment).

## Trigger

The household wants its care and kinship structure to be explicit.

## Preconditions

Both people exist in the same household and the actor is a manager.

## Main Flow

1. The actor picks two people and a relationship type.
2. DomusMind records the relationship in the household structure.

## Alternative Flows

- The actor removes an existing relationship; it is no longer part of the household structure.

## Failure Conditions

- The actor is not a manager: rejected (decided: Q-0024).
- Either person does not exist: rejected.
- Both sides are the same person: rejected.
- The same type already exists for the pair: rejected.

## Postconditions

The relationship is part of the household structure. Only managers manage relationships (decided: Q-0024). A care relationship is a link between two people, not a person role (decided: Q-0021).
