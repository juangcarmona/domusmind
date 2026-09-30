---
id: JRN-PRODUCT-COORDINATE-THE-WEEK
type: journey
title: "Coordinate the household's week"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
steps:
  - use-case: "UC-AGENDA-NAVIGATE"
  - use-case: "UC-AREAS-REVIEW-OWNERSHIP"
  - use-case: "UC-CALENDAR-SCHEDULE-PLAN"
  - use-case: "UC-TASKS-ASSIGN-TASK"
  - use-case: "UC-LISTS-SCHEDULE-ITEM"
provenance:
  source: "docs/_legacy/00_product/strategy.md (Positioning, Gains); docs/_legacy/00_product/experience.md (Success Criteria, Calendar Coordination); openspec/specs/web-app/spec.md (Agenda Time Modes, Agenda Scope, Areas Surface); docs/_legacy/00_product/public-site.md (Proof Blocks: family week coordination)"
  confidence: "low"
  recovered-from: "inference"
---

## Intended Outcome

The household sees the whole week clearly, spots busy days and unowned work early, and distributes what needs doing before it becomes friction (observed: docs/_legacy/00_product/strategy.md, Positioning "daily and weekly coordination", Gains "weekly coordination becomes easier to understand", "the household reacts less and anticipates more"; docs/_legacy/00_product/experience.md, Success Criteria "Agenda week planning feels calm and fast").

## Entry Conditions

- The household exists with people and some plans, tasks or routines for the coming week.

## Journey Narrative

1. A person moves the Agenda to Week mode, which starts on the household's first day of week (observed: web-app spec, Agenda Time Modes).
2. They check the Areas surface for areas without an owner, which are listed first (observed: web-app spec, Areas Surface).
3. They add the plans the week still needs (inferred).
4. They assign open tasks to people so ownership is visible (inferred from strategy.md, Gains "ownership becomes visible instead of implied").
5. They give preparation list items a due date so they show up on the right day (inferred from experience.md, Lists "school preparation with due dates").

## Variants and Branches

- The household plans the week's meals and derives a shopping list (see JRN-MEALS-PLAN-THE-WEEK).
- A person switches to one person's week to check their load (observed: web-app spec, Agenda Scope).
- Month mode is used to see load across weeks (observed: web-app spec, Agenda Time Modes).

## Completion Conditions

The week's plans, owned tasks and dated preparation items are visible in the week view (inferred). The sources describe weekly coordination as an outcome but not as a step-by-step path; this composition is inferred and needs review.
