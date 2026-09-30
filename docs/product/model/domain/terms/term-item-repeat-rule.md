---
id: TERM-ITEM-REPEAT-RULE
type: domain-term
title: "Item Repeat Rule"
status: draft
defined-in: BC-LISTS
synonyms:
  - "repeat"
  - "RepeatRule"
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
  - TERM-PLAN
provenance:
  source: "docs/_legacy/04_contexts/shared-lists-item-model.md (Group 4, Invariants 2-3, Key ownership boundary); openspec/specs/lists/spec.md (Item Temporal Assignment: repeat rule scenario, Notes: repeat independence)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Definition

A lightweight recurrence on a list item that is itself sufficient to place the item in the Agenda on each occurrence, with or without a due date. When a due date is also set, the due date anchors the first (or current) occurrence (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Invariants 2-3; openspec/specs/lists/spec.md, Notes: repeat independence).

## Distinguish From

- **Plan recurrence**: the item repeat rule is a projection hint owned by Lists; it is not the recurrence rule of a plan and does not schedule anything in the Calendar sense (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Key ownership boundary).

## Usage

The format of a repeat rule (which recurrence patterns are supported) is not specified in the sources; the code stores it as free text (observed: src/backend/DomusMind.Domain/Lists/ListItem.cs; inferred: gap).
