---
id: UC-FAMILY-DISABLE-MEMBER-ACCESS
type: use-case
title: "Disable a person's access"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-MANAGERS-ADMINISTER
  - BR-FAMILY-MANAGER-KEEPS-OWN-ACCESS
uses-terms:
  - TERM-MEMBER-ACCESS
  - TERM-MANAGER
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (Phase 1 slice set, Permission rules, MemberAccessStatus)"
  confidence: high
  recovered-from: documentation
---

## Goal

A person can no longer sign in, while remaining part of the household (observed: openspec/specs/family/spec.md, Member Access Provisioning).

## Trigger

A manager decides a person should stop using DomusMind for now.

## Preconditions

The person has an Active account, the actor is a manager and the person is not the manager themselves.

## Main Flow

1. The manager chooses to disable the person's access.
2. DomusMind disables the account; the person's access becomes Disabled.

## Alternative Flows

None known.

## Failure Conditions

- The manager targets themselves: rejected.
- The actor is not a manager: rejected.

## Postconditions

The person stays in the household roster with Disabled access.
