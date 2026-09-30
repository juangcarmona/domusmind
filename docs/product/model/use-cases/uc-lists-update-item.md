---
id: UC-LISTS-UPDATE-ITEM
type: use-case
title: "Edit an item's name, quantity or note"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-LISTS
governed-by:
  - BR-LISTS-ITEM-NAME-REQUIRED
  - BR-LISTS-ARCHIVED-READ-ONLY
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Item Update); docs/_legacy/00_product/surfaces/lists.md (Inspector: Title, Metadata); src/backend/DomusMind.Domain/Lists/SharedList.cs (UpdateItem); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Correct or enrich an item's base details.

## Trigger

A person edits an item in the inspector.

## Preconditions

The item exists.

## Main Flow

1. The person changes the name, quantity or note.
2. The product saves the changed fields only.

## Alternative Flows

- The person clears the quantity: the item no longer has a quantity.

## Failure Conditions

- No field is provided: the update is rejected.
- The name is emptied: the update is rejected.
- The list is archived: the change is rejected; archived lists are read-only until restored (decided: Q-0031).

## Postconditions

The item shows the new details; its checked state, importance, temporal fields and position are unchanged.
