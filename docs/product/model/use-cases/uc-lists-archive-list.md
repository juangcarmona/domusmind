---
id: UC-LISTS-ARCHIVE-LIST
type: use-case
title: "Archive a list"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-ARCHIVE-STATE
uses-terms:
  - TERM-LIST
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (List Archive); docs/_legacy/00_product/surfaces/lists.md (Lifecycle); src/backend/DomusMind.Domain/Lists/SharedList.cs (Archive)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Put away a list that is no longer in active use without losing it.

## Trigger

A person decides a list is no longer needed for now.

## Preconditions

The list is active.

## Main Flow

1. The person archives the list.
2. The product removes it from the active lists, keeping all its items and states.

## Alternative Flows

None.

## Failure Conditions

- The list is already archived: the archive is rejected.

## Postconditions

The list is archived and hidden from the default list view; its data is intact.
