---
id: SB-LISTS-SET-REMINDER-ONLY
type: structured-behaviour
title: "A reminder alone is enough to appear in the Agenda"
status: draft
illustrates:
  - UC-LISTS-SCHEDULE-ITEM
  - BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT
given:
  - "a list item has no temporal fields"
when: "a person sets only a reminder at an absolute date and time"
then:
  - "the item becomes eligible to appear in the Agenda at the reminder time"
  - "no due date is required"
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Temporal Assignment; Scenario: Household sets a reminder without a due date)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Reminder does not depend on a due date.

## Boundaries

Does not describe how the reminder notifies anyone; the sources only define its Agenda effect.
