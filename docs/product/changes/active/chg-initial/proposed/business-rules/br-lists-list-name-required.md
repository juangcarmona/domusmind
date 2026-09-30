---
id: BR-LISTS-LIST-NAME-REQUIRED
type: business-rule
title: "A list must have a non-empty name"
status: draft
applies-to:
  - UC-LISTS-CREATE-LIST
  - UC-LISTS-UPDATE-LIST
uses-terms:
  - TERM-LIST
provenance:
  source: "openspec/specs/lists/spec.md (List Creation); docs/_legacy/04_contexts/shared-lists.md (Invariants: SharedList); src/backend/DomusMind.Domain/Lists/ValueObjects/ListName.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A list's name is required and can never be empty; it is the only input required to create a list (observed: openspec/specs/lists/spec.md, List Creation; docs/_legacy/04_contexts/shared-lists.md, Invariants). The code additionally trims the name and caps it at 200 characters (observed: ListName.cs).

## Rationale

Creating a list must be frictionless, with no required metadata beyond a name (observed: docs/_legacy/00_product/surfaces/lists.md, Creation Model).

## Examples

- Creating a list named "Groceries" with no other details succeeds.
- Creating or renaming a list to an empty or blank name is rejected.

## Exceptions

None.
