---
id: BR-AGENDA-LIST-ITEMS-HOUSEHOLD-ROW
type: business-rule
title: "Projected list items sit in the household's shared row"
status: draft
applies-to:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
uses-terms:
  - "TERM-LIST-ITEM"
  - "TERM-AGENDA-SCOPE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Projected List Item Scope Placement Rules)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

In V1 list items are household-scoped, so in Household scope projected list items appear in the shared household row or lane, never in a person's row; in a Month cell they add to the day's count; in Member scope they appear in the non-timed section, ordered by importance then due date (observed: agenda.md).

## Rationale

List items have no per-person scoping in V1 (observed: agenda.md).

## Examples

- "Renew car insurance" due Thursday appears in the shared row of Thursday's board (inferred example).

## Exceptions

None.
