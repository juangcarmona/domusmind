---
id: UC-AREAS-ADD-SUPPORT
type: use-case
title: Add Support to an Area
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by:
- BR-AREAS-ANY-MEMBER-MANAGES
- BR-AREAS-SAME-HOUSEHOLD-PEOPLE
- BR-AREAS-NO-INACTIVE-ASSIGNEE
- BR-AREAS-UNIQUE-SUPPORT
- BR-AREAS-OWNER-NOT-SUPPORT
- BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE
uses-terms:
- TERM-AREA
- TERM-AREA-SUPPORT
- TERM-MEMBER
provenance:
  source: "openspec/specs/areas/spec.md (Secondary Owner Assignment); docs/_legacy/00_product/surfaces/areas.md (Interaction); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (AssignSecondaryOwner); interview: product owner decision Q-0014 (E-0158); interview: product owner decision Q-0017 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

An Area has backup or shared accountability coverage.

## Trigger

A person adds a Support person inline in the Area detail (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).

## Preconditions

The Area exists; the chosen person belongs to the household and is not already Support of this Area. Any Household Member may do this (decided: Q-0014).

## Main Flow

1. The person selects an Area.
2. They add a household person as Support.
3. The product adds that person to the Area's Support; the Owner is unchanged (observed: openspec/specs/areas/spec.md, Secondary Owner Assignment).

## Alternative Flows

None.

## Failure Conditions

- The person is already Support of the Area: rejected (observed: openspec/specs/areas/spec.md, scenario Duplicate secondary owner is rejected).
- The person is not part of the household: rejected (observed: openspec/specs/areas/spec.md).
- The person is the Area's Owner: rejected (decided: Q-0017).

## Postconditions

The chosen person is Support of the Area; the Owner and the other Support people are unchanged.
