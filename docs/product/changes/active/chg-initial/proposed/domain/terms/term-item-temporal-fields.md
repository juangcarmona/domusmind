---
id: TERM-ITEM-TEMPORAL-FIELDS
type: domain-term
title: "Item Temporal Fields"
status: draft
defined-in: BC-LISTS
synonyms:
  - "temporal capability"
  - "due date, reminder, repeat"
uses-terms:
  - TERM-LIST-ITEM
  - TERM-ITEM-REPEAT-RULE
  - TERM-AGENDA
provenance:
  source: "openspec/specs/lists/spec.md (Item Temporal Assignment, Item Temporal Clearing); docs/_legacy/04_contexts/shared-lists-item-model.md (Group 4 - Temporal, Invariants 2-4); docs/_legacy/00_product/surfaces/lists.md (Item Capability Model)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

The three independently optional time references a list item may carry: a **due date** (a calendar date), a **reminder** (an absolute date and time) and a **repeat rule**. Having any one of them makes the item eligible to appear in the Agenda (observed: openspec/specs/lists/spec.md, Item Temporal Assignment; docs/_legacy/04_contexts/shared-lists-item-model.md, Group 4).

## Distinguish From

- **Plan reminder**: on a plan, reminders are relative (e.g. 30 minutes before); on a list item the reminder is an absolute date-time (observed: docs/_legacy/03_domain/ubiquitous-language.md, Reminder).
- **Scheduling a plan**: setting temporal fields does not create a plan or a task (observed: openspec/specs/lists/spec.md, Item Temporal Assignment).

## Usage

Managed from the Time section of the item inspector; shown on the row as a small date cue, in a warning colour when overdue, with a bell when a reminder is set (observed: docs/_legacy/00_product/surfaces/lists.md, Inspector: Time, UX Grammar).
