---
id: BR-LISTS-NEW-ITEM-UNCHECKED-APPENDED
type: business-rule
title: "A new item starts unchecked at the end of the list"
status: draft
applies-to:
  - UC-LISTS-ADD-ITEM
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Item Addition); src/backend/DomusMind.Domain/Lists/SharedList.cs (AddItem)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A newly added item is always unchecked and receives the next position in the list's order. Importance and temporal fields are not set when an item is added; they are applied afterwards (observed: openspec/specs/lists/spec.md, Item Addition).

## Rationale

Capture must be faster than remembering; enrichment is progressive (observed: docs/_legacy/00_product/surfaces/lists.md, Core Principles).

## Examples

- Adding "Eggs" to a list of three items places it fourth, unchecked.

## Exceptions

None.
