---
id: FR-LISTS-OPEN-IN-LISTS-FROM-AGENDA
type: functional-requirement
title: "Open a projected list item in Lists"
status: draft
derived-from:
  - UC-LISTS-SEE-ITEMS-IN-AGENDA
  - BR-AGENDA-PROJECTION-READ-ONLY
verification:
  - scenario: "Selecting a projected list item in the Agenda shows its title, due date, checked state and list name with a single Open in Lists action and no edit affordance; choosing Open in Lists shows the item in its list."
uses-terms:
  - TERM-PROJECTED-LIST-ITEM
  - TERM-AGENDA
provenance:
  source: "docs/_legacy/00_product/surfaces/lists.md (Relationship with Agenda, UX Grammar: Edit path)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

When a person selects a projected list item in the Agenda, the product MUST show a read-only view with the item's title, due date, checked state and list name, offering only an action to open it in its list. The Agenda MUST NOT offer any edit of the item.

## Rationale

Editing only in Lists preserves the ownership model.
