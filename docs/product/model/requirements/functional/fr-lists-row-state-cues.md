---
id: FR-LISTS-ROW-STATE-CUES
type: functional-requirement
title: "Item rows show their state compactly"
status: draft
derived-from:
  - UC-LISTS-OPEN-LIST
verification:
  - scenario: "An overdue starred item with a reminder shows a filled star, a warning-coloured date cue and a bell, in a row of the same compact height as a name-only item."
  - scenario: "A checked item's title is struck through and the row de-emphasised."
uses-terms:
  - TERM-ITEM-IMPORTANCE
  - TERM-ITEM-TEMPORAL-FIELDS
  - TERM-CHECKED-ITEM
provenance:
  source: "docs/_legacy/00_product/surfaces/lists.md (Row Model, UX Grammar - Lists Surface (Locked))"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

Each item row MUST show one item with a toggle and its title, and MAY show short secondary metadata (quantity, short note). A star on the trailing edge MUST be filled when the item is important and unfilled otherwise. A due date MUST appear as a small cue below the title, in a warning colour when overdue, with a bell when a reminder is set. A checked item MUST be struck through and de-emphasised. No state may enlarge a row without the person selecting it.

## Rationale

Density and recognisable state make lists fast to scan.
