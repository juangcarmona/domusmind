---
id: UC-AREAS-REMOVE-SUPPORT
type: use-case
title: Remove Support from an Area
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by:
- BR-AREAS-ANY-MEMBER-MANAGES
- BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE
uses-terms:
- TERM-AREA
- TERM-AREA-SUPPORT
provenance:
  source: "openspec/specs/areas/spec.md (Secondary Owner Removal); docs/_legacy/00_product/surfaces/areas.md (Interaction); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (RemoveSecondaryOwner); interview: product owner decision Q-0014 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

A person no longer provides Support for an Area.

## Trigger

A person removes a Support person inline in the Area detail (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).

## Preconditions

The Area has at least one Support person. Any Household Member may do this (decided: Q-0014).

## Main Flow

1. The person selects an Area.
2. They remove one Support person.
3. The product removes that person from the Area's Support (observed: openspec/specs/areas/spec.md, Secondary Owner Removal).

## Alternative Flows

None.

## Failure Conditions

None stated by the sources.

## Postconditions

The removed person is no longer Support; the Owner and all other Support people are unchanged.
