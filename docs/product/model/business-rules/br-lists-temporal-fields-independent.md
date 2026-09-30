---
id: BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT
type: business-rule
title: "Due date, reminder and repeat are independently optional"
status: draft
applies-to:
  - UC-LISTS-SCHEDULE-ITEM
  - UC-LISTS-CLEAR-ITEM-SCHEDULE
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
  - TERM-ITEM-REPEAT-RULE
provenance:
  source: "openspec/specs/lists/spec.md (Item Temporal Assignment, Agenda Projection, Notes: repeat independence); docs/_legacy/04_contexts/shared-lists-item-model.md (Invariants 2-4, Valid State Combinations); docs/_legacy/00_product/surfaces/lists.md (Temporal field independence); docs/_legacy/04_contexts/shared-lists.md (Invariants); src/backend/DomusMind.Domain/Lists/ListItem.cs (SetTemporal); interview: product owner decision Q-0030 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Any combination of due date, reminder and repeat is valid; none requires another. A repeat rule alone and a reminder alone (absolute date-time) each suffice. When both repeat and due date are set, the due date anchors the first (or current) occurrence. Each field can be cleared on its own (decided: Q-0030). If the due date is cleared while repeat remains, the item stays Agenda-eligible through repeat (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Invariants 2-4; openspec/specs/lists/spec.md, Notes: repeat independence).

Resolved conflict: a legacy feature spec once required a due date for repeat; the openspec records it as stale (observed: openspec/specs/lists/spec.md, Notes). The Lists surface's inspector text ("clearing due date removes eligibility if no reminder is set") omits repeat and is superseded by this rule (observed: docs/_legacy/00_product/surfaces/lists.md, Inspector: Time).

## Rationale

Household items recur or need a nudge without always having a single due date.

## Examples

- "Water the plants" with only a weekly repeat appears in the Agenda each week.
- "Call the dentist" with only a reminder at 09:00 appears at that time.

## Exceptions

None.
