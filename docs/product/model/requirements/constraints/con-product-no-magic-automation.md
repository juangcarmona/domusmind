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

DomusMind does not promise or perform hidden automation. Household records (plans, tasks, routines, list items, area ownership) come into existence only through an explicit household decision; intelligence must strengthen household structure without bypassing its rules.

Public claims must not promise AI capabilities the product cannot deliver.

The assistant responds only after explicit `@dm` invocation and uses information the invoking member may access. For a supported action request, it may ask clarifying questions and stage a proposal in Inbox. It cannot directly invoke a household domain command. Each resulting record requires explicit Inbox approval and the owning area's authorization and validation. Assistant proposals are not created from private Chat content.

## Rationale

Trust in a shared household system depends on the household recognizing every item as something someone decided. The product's value is shared clarity and visible responsibility.

## Consequences

- Tasks are created explicitly and never from plans or meals automatically; imported external calendar entries never become plans; list items never become tasks; routines are shown, not turned into tasks.
- AI-assisted interpretation may propose Plan creation or updates, Tasks, Plan reminders, Timeline Notes and List Item additions or updates through Inbox, but every household record still requires a person's explicit decision.
- Chats answers are grounded in authorized information and does not write to domain records; action proposals require Inbox review.
- Provider routing is ordered, visible and limited to retryable provider failures; no fallback conceals a valid answer or an authorization or domain-validation result.
- Shopping lists generated from meal plans happen only when the household asks for them.
