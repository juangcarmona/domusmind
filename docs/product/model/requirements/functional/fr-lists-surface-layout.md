---
id: FR-LISTS-SURFACE-LAYOUT
type: functional-requirement
title: "Lists surface: switcher, active list, quick add and inspector"
status: draft
derived-from:
  - UC-LISTS-BROWSE-LISTS
  - UC-LISTS-OPEN-LIST
  - UC-LISTS-ADD-ITEM
verification:
  - scenario: "On desktop a person sees the list switcher, the active list and, after selecting an item, an inline inspector with status, title, importance, time, quantity, note and remove; on mobile the switcher opens as a drawer and item detail as a bottom sheet."
  - scenario: "Checked items of the active list are collapsed under Completed (N) and can be expanded."
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
  - TERM-CHECKED-ITEM
provenance:
  source: "docs/_legacy/00_product/surfaces/lists.md (Surface Structure, Layout Zones, Inspector = Command Surface, Completed Items, List Switcher Behavior)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The Lists surface MUST provide a list switcher (every list with name, unchecked count and optional Area or plan cue; empty lists stay visible; the active list is clearly marked), the active list (header with name, unchecked count and optional Area and plan chips; unchecked items first; checked items collapsed under "Completed (N)"), an always visible quick add, and an item inspector that is the command surface for every item capability (status, title, importance, time, quantity, note, remove). On desktop the inspector is an inline panel; on mobile the switcher is a drawer and the inspector a bottom sheet. Neither adding nor editing an item may require a modal or a separate page.

## Rationale

The surface is optimised for scan speed, quick capture and quick toggling.
