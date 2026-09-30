---
id: UC-AREAS-ASSIGN-OWNER
type: use-case
title: Set or change an Area's Owner
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
- TERM-RESPONSIBILITY-TRANSFER
provenance:
  source: "openspec/specs/areas/spec.md (Primary Owner Assignment); docs/_legacy/00_product/surfaces/areas.md (Interaction); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (AssignPrimaryOwner); interview: product owner decision Q-0012 (E-0158); interview: product owner decision Q-0014 (E-0158); openspec/specs/areas/spec.md (Responsibility Transfer, Notes); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (TransferPrimaryOwner); interview: product owner review decision (merges)"
  confidence: high
  recovered-from: documentation
---

## Goal

An Area has a single accountable person. Giving an Owner to an unowned Area is plain assignment; changing the Owner of an Area that already has one is an explicit, traceable handover of accountability (a Responsibility Transfer). Both are one household action (decided: Q-0012).

## Trigger

A person uses the inline change-Owner affordance in the Area detail (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction; openspec/specs/areas/spec.md, Responsibility Transfer).

## Preconditions

The Area exists; the chosen person belongs to the same household. Any Household Member may do this (decided: Q-0014).

## Main Flow

1. The person selects an Area.
2. They choose a household person as Owner.
3. The product makes that person the Owner; the Area moves from unowned to owned (observed: openspec/specs/areas/spec.md, Primary Owner Assignment).

## Alternative Flows

- Changing an existing Owner is a traceable transfer: when the Area already has an Owner, the same change-Owner action makes the new person the Owner, the previous Owner no longer holds the role, the Area stays active, and the handover is traceable afterwards (decided: Q-0012; observed: openspec/specs/areas/spec.md, scenarios Primary owner is replaced and Household transfers Area ownership).
- The chosen person was Support: they stop being Support (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs).

## Failure Conditions

- The chosen person is not part of the household: the assignment or transfer is rejected and any existing Owner is unchanged (observed: openspec/specs/areas/spec.md).

## Postconditions

The Area has exactly one Owner; when an existing Owner was changed, the handover is traceable. How the household sees the history of transfers is not specified.
