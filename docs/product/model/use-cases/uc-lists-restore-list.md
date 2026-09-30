---
id: UC-LISTS-RESTORE-LIST
type: use-case
title: "Restore an archived list"
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
  source: "openspec/specs/lists/spec.md (List Restore); src/backend/DomusMind.Domain/Lists/SharedList.cs (Restore)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Bring an archived list back into use exactly as it was.

## Trigger

A person needs an archived list again.

## Preconditions

The list is archived.

## Main Flow

1. The person restores the list.
2. The product returns it to the active lists with every item in the state it was archived in.

## Alternative Flows

None.

## Failure Conditions

- The list is not archived: the restore is rejected.

## Postconditions

The list is active again and behaves like any other active list.
