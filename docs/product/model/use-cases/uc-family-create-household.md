---
id: UC-FAMILY-CREATE-HOUSEHOLD
type: use-case
title: "Create a household"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-HOUSEHOLD-NAME-REQUIRED
  - BR-FAMILY-CREATOR-IS-FIRST-MANAGER
uses-terms:
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/family/spec.md (Household Creation, NOTES N4, N8); docs/_legacy/04_contexts/family.md (Commands: CreateFamily); src/backend/DomusMind.Domain/Family/Family.cs (Create); interview: product owner decision Q-0022 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

A new household exists in DomusMind, ready to receive its people (observed: openspec/specs/family/spec.md, Household Creation).

## Trigger

Someone starts using DomusMind for their home. Whether this happens automatically during onboarding or by explicit action is not specified (observed: openspec family spec, NOTE N8).

## Preconditions

None.

## Main Flow

1. The person gives the household a name.
2. DomusMind creates the household with a stable identity and no pets or relationships yet, and makes the person who created it its first manager (decided: Q-0022).

## Alternative Flows

None known.

## Failure Conditions

- The name is empty: the household is not created and a validation error is shown (observed: scenario "Household creation fails with an empty name").

## Postconditions

The household exists with its name, and the person who created it is its first manager (decided: Q-0022); the rest of the product can start referring to it. The specs had left the first manager unspecified (observed: NOTE N4).
