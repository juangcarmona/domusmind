---
id: JRN-PRODUCT-WHAT-MATTERS-TODAY
type: journey
title: "Handle what matters today across the household"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
steps:
  - use-case: "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - use-case: "UC-AGENDA-INSPECT-ENTRY"
  - use-case: "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - use-case: "UC-AGENDA-NAVIGATE"
  - use-case: "UC-TASKS-COMPLETE-TASK"
  - use-case: "UC-LISTS-TOGGLE-ITEM"
  - use-case: "UC-AGENDA-CREATE-FROM-AGENDA"
provenance:
  source: "docs/_legacy/00_product/strategy.md (Positioning, Gains, Differentiators); docs/_legacy/00_product/experience.md (Core Experience Principles, Agenda, Routines); openspec/specs/web-app/spec.md (Agenda Default Entry State, Agenda Item Priority Ordering, Agenda Selection and Inspection, Agenda Projected List Items); docs/_legacy/00_product/surface-system.md (Interaction Grammar); docs/_legacy/00_product/surfaces/agenda.md (Default Entry State, Scope, Interaction Grammar); docs/_legacy/00_product/experience.md (Agenda); interview: product owner review decision (merges)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intended Outcome

Everyone can see what matters today, for the household and for one person, and the household acts on it without one person reminding the others: tasks get done, list items get checked, missing items get captured, all without leaving the Agenda (observed: docs/_legacy/00_product/strategy.md, Gains "everyone can see what matters today", Positioning "the constant question of what matters today"; docs/_legacy/00_product/experience.md, Core Experience Principles "Action first"; agenda.md, Default Entry State, Anti-Patterns "navigate to a separate page").

## Entry Conditions

- The household exists and holds plans, tasks, routines or dated list items for today.
- The person belongs to the household and opens DomusMind.

## Journey Narrative

1. A person opens DomusMind on today's household Agenda: the shared row and one row per person, with overdue items and today's tasks first (observed: openspec/specs/web-app/spec.md, Agenda Default Entry State, Agenda Item Priority Ordering; agenda.md).
2. They select an entry to see its detail in the inspector or bottom sheet without leaving the Agenda (observed: openspec/specs/web-app/spec.md, Agenda Selection and Inspection; agenda.md).
3. They tap a person to switch to that person's scope, keeping Day and today, and on the person's timeline spot a gap or conflict (observed: agenda.md, Scope, Timeline).
4. They complete a task that is done (inferred: completion stays lightweight, docs/_legacy/00_product/experience.md, Routines).
5. For a dated list item they go to the list through "Open in Lists" and check it off there (observed: openspec/specs/web-app/spec.md, Agenda Projected List Items).
6. They add anything missing for today, such as a plan or a task, from the Agenda itself: clicking an empty slot or pressing Add prefills the person, date and time (observed: docs/_legacy/00_product/surface-system.md, Capture stays local; docs/_legacy/00_product/experience.md, Agenda "write-capable in-place"; agenda.md).

## Variants and Branches

- The person moves to Week when Day is not enough, or to Month for load awareness (observed: agenda.md).
- Missed routine work surfaces quietly rather than as alarms (observed: experience.md, Routines).
- Imported Outlook entries are visible only in a person's own scope and cannot be acted on (observed: web-app spec, Agenda External Calendar Entries).

## Completion Conditions

The person has the answer to "what matters today?": today's tasks and list items reflect what was done, any new item appears in the Agenda, and the household picture shows what still needs attention (inferred).
