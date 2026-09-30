---
id: JRN-PRODUCT-WHAT-MATTERS-TODAY
type: journey
title: "Handle what matters today across the household"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
steps:
  - use-case: "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - use-case: "UC-AGENDA-INSPECT-ENTRY"
  - use-case: "UC-TASKS-COMPLETE-TASK"
  - use-case: "UC-LISTS-TOGGLE-ITEM"
  - use-case: "UC-AGENDA-CREATE-FROM-AGENDA"
provenance:
  source: "docs/_legacy/00_product/strategy.md (Positioning, Gains, Differentiators); docs/_legacy/00_product/experience.md (Core Experience Principles, Agenda, Routines); openspec/specs/web-app/spec.md (Agenda Default Entry State, Agenda Item Priority Ordering, Agenda Selection and Inspection, Agenda Projected List Items); docs/_legacy/00_product/surface-system.md (Interaction Grammar)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intended Outcome

Everyone can see what matters today, and the household acts on it without one person reminding the others: tasks get done, list items get checked, missing items get captured (observed: docs/_legacy/00_product/strategy.md, Gains "everyone can see what matters today", Positioning "the constant question of what matters today"; docs/_legacy/00_product/experience.md, Core Experience Principles "Action first").

## Entry Conditions

- The household exists and holds plans, tasks, routines or dated list items for today.

## Journey Narrative

1. A person opens DomusMind on today's household Agenda, with overdue items and today's tasks first (observed: openspec/specs/web-app/spec.md, Agenda Default Entry State, Agenda Item Priority Ordering).
2. They select an entry to see its detail without leaving the Agenda (observed: openspec/specs/web-app/spec.md, Agenda Selection and Inspection).
3. They complete a task that is done (inferred: completion stays lightweight, docs/_legacy/00_product/experience.md, Routines).
4. For a dated list item they go to the list through "Open in Lists" and check it off there (observed: openspec/specs/web-app/spec.md, Agenda Projected List Items).
5. They add anything missing for today, such as a plan or a task, from the Agenda itself (observed: docs/_legacy/00_product/surface-system.md, Capture stays local; docs/_legacy/00_product/experience.md, Agenda "write-capable in-place").

## Variants and Branches

- The person switches to one person's scope to see that person's day in depth (observed: web-app spec, Agenda Scope); the Agenda area's JRN-AGENDA-COORDINATE-THE-DAY details the within-Agenda path.
- Missed routine work surfaces quietly rather than as alarms (observed: experience.md, Routines).
- Imported Outlook entries are visible only in a person's own scope and cannot be acted on (observed: web-app spec, Agenda External Calendar Entries).

## Completion Conditions

Today's tasks and list items reflect what was done, and the household picture shows what still needs attention (inferred).
