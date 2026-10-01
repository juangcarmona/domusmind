---
id: SB-LISTS-SET-REPEAT-ONLY
type: structured-behaviour
title: "A repeat rule alone is enough to appear in the Agenda"
status: draft
illustrates:
  - UC-LISTS-SCHEDULE-ITEM
  - BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT
given:
  - "a list item exists"
when: "a person sets a repeat rule on it"
then:
  - "the item becomes eligible to appear in the Agenda on each occurrence"
  - "the repeat rule alone is sufficient for that"
uses-terms:
  - TERM-ITEM-REPEAT-RULE
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Temporal Assignment; Scenario: Household sets a repeat rule)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Repeat does not require a due date.

## Boundaries

Does not define which repeat patterns exist; the sources are silent.
