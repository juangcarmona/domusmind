---
id: UC-FAMILY-MANAGE-MEMBER-LIFECYCLE
type: use-case
title: "Track a person's stay in the household"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-MEMBER-LIFECYCLE
uses-terms:
  - TERM-MEMBER
provenance:
  source: "docs/_legacy/04_contexts/family-member-management.md (MemberStatus, MembershipPeriod, MemberKind, ResidencyType, Groups A-B)"
  confidence: low
  recovered-from: documentation
---

## Goal

Planned: V1.1. The household records temporary and changing presence: expected arrivals, guests with an end date, people who have left, and archived history (observed: family-member-management.md, MemberStatus, MembershipPeriod, Group A register-temporary-member, Group B).

## Trigger

Someone is about to arrive, is staying for a while, or has left.

## Preconditions

The person is on the roster, or is being added as temporary.

## Main Flow

1. The manager adds a temporary person with a start and end date, or sets those dates on an existing person.
2. The manager activates, deactivates or archives the person as their situation changes.

## Alternative Flows

None known.

## Failure Conditions

- A move not allowed by the lifecycle, a guest without an end date, or an end date before the start: rejected.

## Postconditions

Archived people are hidden from all operational views; inactive people cannot get new tasks or plans (observed: family-member-management.md, Invariants). Roadmap proposal only.
