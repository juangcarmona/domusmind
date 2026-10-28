---
id: UC-FAMILY-ENABLE-MEMBER-ACCESS
type: use-case
title: "Re-enable a person's access"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-MANAGERS-ADMINISTER
uses-terms:
  - TERM-MEMBER-ACCESS
  - TERM-MANAGER
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (Phase 1 slice set, Permission rules, MemberAccessStatus)"
  confidence: high
  recovered-from: documentation
---

## Goal

A person whose application access was disabled can use DomusMind again.

## Trigger

A manager decides a disabled person should use DomusMind again.

## Preconditions

The person has Disabled access and the actor is a manager.

## Main Flow

1. The manager chooses to enable the person's access.
2. DomusMind re-enables application access for the existing bound provider identity; the person's access becomes Active.

## Alternative Flows

None known.

## Failure Conditions

- The actor is not a manager: rejected.

## Postconditions

The person can sign in again.
