---
id: UC-AREAS-TRANSFER-OWNERSHIP
type: use-case
title: Transfer an Area to another Owner
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
- TERM-RESPONSIBILITY-TRANSFER
provenance:
  source: "openspec/specs/areas/spec.md (Responsibility Transfer, Notes); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (TransferPrimaryOwner); interview: product owner decision Q-0012 (E-0158); interview: product owner decision Q-0014 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

Accountability for an Area is handed explicitly from its current Owner to another person.

## Trigger

A person uses the change-Owner affordance on an Area that already has an Owner (decided: Q-0012; observed: openspec/specs/areas/spec.md, Responsibility Transfer; docs/_legacy/00_product/surfaces/areas.md, Interaction).

## Preconditions

The Area has an Owner; the new Owner belongs to the same household. Any Household Member may do this (decided: Q-0014).

## Main Flow

1. The person selects an owned Area.
2. They transfer ownership to another household person.
3. The product makes the new person the Owner, the previous Owner no longer holds the role, and the Area stays active (observed: openspec/specs/areas/spec.md, scenario Household transfers Area ownership).

## Alternative Flows

None.

## Failure Conditions

- The new person is not part of the household: the transfer is rejected and the existing Owner is unchanged (observed: openspec/specs/areas/spec.md).

## Postconditions

The Area has exactly one Owner, the new one, and the handover is traceable.

Assigning an Owner and transferring ownership are one household action: changing an existing Owner is a traceable transfer, while giving an unowned Area its first Owner is plain assignment (decided: Q-0012). How the household sees the history of transfers is not specified.
