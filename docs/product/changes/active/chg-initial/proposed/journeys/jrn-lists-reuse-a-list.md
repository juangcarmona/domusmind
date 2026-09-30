---
id: JRN-LISTS-REUSE-A-LIST
type: journey
title: "Reuse a household list across occasions"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
steps:
  - use-case: "UC-LISTS-CREATE-LIST"
  - use-case: "UC-LISTS-ADD-ITEM"
  - use-case: "UC-LISTS-TOGGLE-ITEM"
  - use-case: "UC-LISTS-ARCHIVE-LIST"
provenance:
  source: "docs/_legacy/00_product/surfaces/lists.md (Lifecycle); openspec/specs/lists/spec.md (Purpose)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intended Outcome

The household keeps one list for a recurring purpose (groceries, packing, restocking) and uses it again and again instead of rebuilding it (observed: docs/_legacy/00_product/surfaces/lists.md, Lifecycle: reuse is more important than one-time completion).

## Entry Conditions

A person in the household has a recurring need to remember or buy a set of things.

## Journey Narrative

1. The person creates the list with a name, standalone or from a plan or Area.
2. Household members add items as they think of them.
3. While using the list (shopping, packing) they check items off.
4. Between uses the list rests with its items, ready for next time; unchecking items makes them relevant again.
5. When the list is no longer needed, it is archived (observed: docs/_legacy/00_product/surfaces/lists.md, Lifecycle).

## Variants and Branches

- An archived list is restored when the need returns (UC-LISTS-RESTORE-LIST).
- Items gain a due date or reminder and appear in the Agenda (UC-LISTS-SCHEDULE-ITEM).

## Completion Conditions

The journey completes when the list is archived; until then the list keeps cycling between active use and rest.
