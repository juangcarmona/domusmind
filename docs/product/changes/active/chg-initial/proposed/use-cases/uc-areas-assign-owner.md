---
id: UC-AREAS-ASSIGN-OWNER
type: use-case
title: Assign an Area's Owner
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by:
- BR-AREAS-ANY-MEMBER-MANAGES
- BR-AREAS-SINGLE-OWNER
- BR-AREAS-SAME-HOUSEHOLD-PEOPLE
- BR-AREAS-NO-INACTIVE-ASSIGNEE
- BR-AREAS-OWNER-NOT-SUPPORT
- BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-MEMBER
provenance:
  source: "openspec/specs/areas/spec.md (Primary Owner Assignment); docs/_legacy/00_product/surfaces/areas.md (Interaction); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (AssignPrimaryOwner); interview: product owner decision Q-0012 (E-0158); interview: product owner decision Q-0014 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

An Area has a single accountable person. Giving an Owner to an unowned Area is plain assignment (decided: Q-0012).

## Trigger

A person uses the inline change affordance for the Owner in the Area detail (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).

## Preconditions

The Area exists; the chosen person belongs to the same household. Any Household Member may do this (decided: Q-0014).

## Main Flow

1. The person selects an Area.
2. They choose a household person as Owner.
3. The product makes that person the Owner; the Area moves from unowned to owned (observed: openspec/specs/areas/spec.md, Primary Owner Assignment).

## Alternative Flows

- The Area already has an Owner: the same change-Owner action replaces the previous Owner, and that change is a traceable Responsibility Transfer (UC-AREAS-TRANSFER-OWNERSHIP) (decided: Q-0012; observed: openspec/specs/areas/spec.md, scenario Primary owner is replaced).
- The chosen person was Support: they stop being Support (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs).

## Failure Conditions

- The chosen person is not part of the household: the assignment is rejected (observed: openspec/specs/areas/spec.md).

## Postconditions

The Area has exactly one Owner.
