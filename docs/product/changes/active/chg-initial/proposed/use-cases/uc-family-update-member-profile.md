---
id: UC-FAMILY-UPDATE-MEMBER-PROFILE
type: use-case
title: "Update my profile"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-MANAGERS-ADMINISTER
uses-terms:
  - TERM-MEMBER-PROFILE
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Member Profile Update); docs/_legacy/04_contexts/family.md (Commands: UpdateMemberProfile); src/backend/DomusMind.Domain/Family/FamilyMember.cs (UpdateProfile); interview: product owner decision Q-0060 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

A person's own contact details are current: preferred name, phone, email and household note (observed: openspec/specs/family/spec.md, Member Profile Update).

## Trigger

The person wants others to see how to call or reach them.

## Preconditions

The person is signed in and belongs to the household.

## Main Flow

1. The person opens their own profile.
2. The person changes their preferred name, phone, email or household note.
3. DomusMind saves the profile.

## Alternative Flows

- A manager updates another person's profile in the same way (observed: openspec family spec, "Managers may also update any member's profile details").
- The person chooses an avatar icon and colour, which are part of the person profile (decided: Q-0060; observed: src/backend/DomusMind.Domain/Family/FamilyMember.cs).

## Failure Conditions

- A non-manager tries to change someone else's profile: rejected (inferred from BR-FAMILY-MANAGERS-ADMINISTER).

## Postconditions

The preferred name, when set, is shown instead of the person's name (decided: Q-0060; observed: FamilyMember.cs, DisplayName).
