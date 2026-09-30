---
id: QR-LISTS-FRICTIONLESS-CAPTURE
type: quality-requirement
title: "Capturing list items is faster than remembering them"
status: draft
quality-attribute: "usability"
applies-to:
  - UC-LISTS-ADD-ITEM
  - UC-LISTS-CREATE-LIST
verification:
  - scenario-ref: "SB-LISTS-SEQUENTIAL-CAPTURE"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "docs/_legacy/00_product/surfaces/lists.md (Core Principles, Quick Add, Creation Model); openspec/specs/lists/spec.md (Item Addition: Sequential item capture)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

Quick add MUST be always visible in the active list, MUST need only an item name, MUST NOT open a modal, and MUST return focus for the next entry so items can be captured in sequence without interruption. Creating a list MUST need only a name (observed: docs/_legacy/00_product/surfaces/lists.md, Quick Add, Creation Model).

## Measurement

Adding several items in a row requires only typing each name and confirming; no dialog, page change or extra field appears between entries, and the input keeps focus after each addition.
