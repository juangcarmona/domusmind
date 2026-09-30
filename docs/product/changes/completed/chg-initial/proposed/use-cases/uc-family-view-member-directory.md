---
id: UC-FAMILY-VIEW-MEMBER-DIRECTORY
type: use-case
title: "View the people directory"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-DIRECTORY-ORDER
uses-terms:
  - TERM-MEMBER-DIRECTORY
  - TERM-MEMBER-ACCESS
  - TERM-MANAGER
provenance:
  source: "openspec/specs/family/spec.md (Household Member Directory); docs/_legacy/04_contexts/family-member-management.md (Phase 1 slice set, MemberDirectoryItemResponse, Sort order); interview: product owner decision Q-0021 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

The person sees everyone in the household, their role, whether they are a manager and their access state, and what the viewer may do with each entry (observed: openspec/specs/family/spec.md, Household Member Directory; family-member-management.md, Phase 1).

## Trigger

The person opens the household's people view.

## Preconditions

The person belongs to the household.

## Main Flow

1. The person opens the directory.
2. DomusMind lists adults, then children, then pets; managers first in each group, then by name.
3. Each entry shows the person's details, access state, whether it is the viewer, and whether the viewer may edit them or grant them access.

## Alternative Flows

- The person opens one entry to see that person's details (observed: family-member-management.md, view-member-details).

## Failure Conditions

None known.

## Postconditions

None; viewing changes nothing.
