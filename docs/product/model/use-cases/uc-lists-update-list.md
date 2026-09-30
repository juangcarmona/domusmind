---
id: UC-LISTS-UPDATE-LIST
type: use-case
title: "Rename or re-link a list"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-LIST-NAME-REQUIRED
  - BR-LISTS-CONTEXT-LINKS-INFORMATIONAL
  - BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT
uses-terms:
  - TERM-LIST
  - TERM-LIST-KIND
  - TERM-AREA
  - TERM-PLAN
provenance:
  source: "openspec/specs/lists/spec.md (List Update); docs/_legacy/04_contexts/shared-lists.md (Commands: RenameSharedList, LinkSharedList, UnlinkSharedList); src/backend/DomusMind.Domain/Lists/SharedList.cs (UpdateMetadata)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Keep a list's name and context accurate.

## Trigger

A person changes the list's name, Area, linked plan or kind.

## Preconditions

The list exists.

## Main Flow

1. The person changes one or more of: name, Area association, linked plan, kind.
2. The product applies only the changed fields and leaves the rest unchanged.

## Alternative Flows

- The person clears the Area association or the plan link: the list is no longer associated, and its items are unaffected.

## Failure Conditions

- Nothing is changed: the update is rejected.
- The new name is empty: the update is rejected.

## Postconditions

The list reflects the new details; its items are untouched.
