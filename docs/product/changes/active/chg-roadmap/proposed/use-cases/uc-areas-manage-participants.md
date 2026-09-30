---
id: UC-AREAS-MANAGE-PARTICIPANTS
type: use-case
title: Add or remove an Area participant
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by: []
uses-terms:
- TERM-AREA
- TERM-AREA-PARTICIPANT
provenance:
  source: "docs/_legacy/04_contexts/responsibilities.md (Commands, Invariants, Slice Mapping); docs/_legacy/00_product/surfaces/areas.md (Data); openspec/specs/areas/spec.md (Notes, Participant role); interview: product owner decision Q-0016 (E-0158)"
  confidence: low
  recovered-from: documentation
---

## Goal

Record who is involved in an Area without being accountable for it.

## Trigger

A person adds or removes a participant on an Area (observed: docs/_legacy/04_contexts/responsibilities.md, Commands).

## Preconditions

The Area exists; the person belongs to the household.

## Main Flow

1. The person adds a household person as participant, or removes one.
2. The product records the change; participants are unique within the Area (observed: docs/_legacy/04_contexts/responsibilities.md, Invariants).

## Alternative Flows

None.

## Failure Conditions

- The person is already a participant: (inferred from uniqueness) rejected.

## Postconditions

Participant detail is shown only in the Area detail when it helps understanding (observed: docs/_legacy/00_product/surfaces/areas.md, Data).

Planned (future), low confidence (decided: Q-0016). The Areas spec excludes participants until a feature spec exists, and the domain code has no participants (observed: openspec/specs/areas/spec.md, Notes, Participant role; src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs).
