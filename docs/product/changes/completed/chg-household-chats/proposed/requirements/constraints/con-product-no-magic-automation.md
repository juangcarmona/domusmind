---
id: CON-PRODUCT-NO-MAGIC-AUTOMATION
type: constraint
title: "No hidden automation: household records are created explicitly"
status: draft
applies-to:
  - "BC-FAMILY"
  - "BC-RESPONSIBILITIES"
  - "BC-CALENDAR"
  - "BC-TASKS"
  - "BC-LISTS"
  - "BC-MEAL-PLANNING"
  - "BC-CHATS"
uses-terms:
  - "TERM-TASK"
  - "TERM-PLAN"
  - "TERM-LIST-ITEM"
provenance:
  source: "docs/_legacy/00_product/strategy.md (Vision, Non-Goals); docs/_legacy/09_roadmap/roadmap.md (V3 - Intelligence and Integrations, Rule); docs/_legacy/00_product/public-site.md (Copy Guardrails); docs/_legacy/03_domain/context-map.md (Tasks from Meal Planning, Event Scheduled); product-owner decision for CHG-HOUSEHOLD-CHATS"
  confidence: medium
  recovered-from: documentation
---

## Constraint

DomusMind does not promise or perform "magic automation". Household records (plans, tasks, routines, list items, area ownership) come into existence only through an explicit household action; any intelligence the product adds grows from clear household structure and must strengthen that structure, never bypass or replace it (observed: docs/_legacy/00_product/strategy.md, Non-Goals "does not promise magic automation", Vision; docs/_legacy/09_roadmap/roadmap.md, V3 Rule "Intelligence comes after clarity").

Public claims must not promise AI capabilities the product cannot deliver (observed: docs/_legacy/00_product/public-site.md, Copy Guardrails).

The Chats assistant answers only after explicit `@dm` invocation, uses authorized DomusMind information, and cannot create or change household records (proposed by CHG-HOUSEHOLD-CHATS). Members use Inbox to review proposed changes.

## Rationale

Trust in a shared household system depends on the household recognising every item as something someone decided (inferred). The product's value is shared clarity and visible responsibility (observed: docs/_legacy/00_product/strategy.md, Non-Goals).

## Consequences

- Corroborated per area: tasks are created explicitly and never from plans or meals automatically; imported external calendar entries never become plans; list items never become tasks; routines are shown, not turned into tasks (observed across the area rules BR-TASKS-TASKS-CREATED-EXPLICITLY, BR-MEALS-NO-AUTOMATIC-TASKS, BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS, BR-LISTS-ITEM-IS-NOT-A-TASK, BR-TASKS-ROUTINES-PROJECTED).
- AI-assisted interpretation may propose Plans, Tasks, Plan reminders and timeline notes through Inbox, but every household record still requires a person's explicit decision. Automatic execution remains out of scope (proposed by CHG-INBOX-ASSISTED-CAPTURE and CHG-INBOX-EXPAND-PROPOSALS; consistent with docs/_legacy/09_roadmap/roadmap.md, V3 Rule).
- The Chats assistant provides read-only answers and directs action requests to Inbox (proposed by CHG-HOUSEHOLD-CHATS).
- The one sanctioned derivation, a shopping list generated from a meal plan, happens only when the household asks for it (observed: the Meal Planning area's shopping-list rules).
