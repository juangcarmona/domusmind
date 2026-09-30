---
id: UC-AREAS-ARCHIVE-AREA
type: use-case
title: Archive an Area
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-RESPONSIBILITIES
governed-by:
- BR-AREAS-ANY-MEMBER-MANAGES
- BR-TASKS-ARCHIVED-AREA-CLEARED
uses-terms:
- TERM-AREA
provenance:
  source: "openspec/specs/areas/spec.md (Area Archiving); docs/_legacy/04_contexts/responsibilities.md (Commands); docs/_legacy/00_product/surfaces/areas.md (Interaction); interview: product owner decision Q-0010 (E-0158); interview: product owner decision Q-0013 (E-0158); interview: product owner decision Q-0014 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

An Area the household no longer uses leaves the active view without being lost.

## Trigger

A person archives an Area (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).

## Preconditions

The Area exists. Any Household Member may do this (decided: Q-0014).

## Main Flow

1. The person archives the Area.
2. The product marks it archived and removes it from the default active view (observed: openspec/specs/areas/spec.md, Area Archiving).
3. The Area remains visible when the archived filter is applied.

## Alternative Flows

None.

## Failure Conditions

None stated by the sources.

## Postconditions

The Area is archived and retained. Tasks and routines that referenced it lose their Area reference (decided: Q-0010).

Archiving an Area is current product intent (decided: Q-0013), although the domain code has no archived state yet (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs); reactivating or deleting an Area is not specified (observed: openspec/specs/areas/spec.md, Notes).
