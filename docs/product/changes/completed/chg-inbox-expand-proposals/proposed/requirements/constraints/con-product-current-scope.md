---
id: CON-PRODUCT-CURRENT-SCOPE
type: constraint
title: The current product covers six household areas and excludes the rest
status: draft
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-EXTERNAL-CALENDAR-ENTRY
  - TERM-CALENDAR-SUBSCRIPTION
provenance:
  source: "docs/_legacy/01_system/system-spec.md (V1 Bounded Contexts, Out of Scope for V1); docs/_legacy/01_system/system-overview.md (Core System Model, System Boundaries); docs/_legacy/09_roadmap/roadmap.md (Out of Scope for the Current Phase); decision Q-0001; GitHub issue #16"
  confidence: medium
  recovered-from: documentation
---

## Constraint

The current product covers the household and its people, areas of responsibility, plans with the Agenda and Phase 1 external calendars, read-only calendar subscriptions, dated family timeline notes, tasks and routines, shared lists, meal planning with recipes, and a review-first Inbox for household notes that can propose Plans, Tasks, Plan reminders and timeline notes (observed: docs/_legacy/01_system/system-spec.md, V1 Bounded Contexts; docs/_legacy/01_system/system-overview.md, Core System Model; meal planning is current product by decision Q-0001; calendar subscriptions are proposed by CHG-CALENDAR-SUBSCRIPTIONS; Inbox scope by CHG-INBOX-ASSISTED-CAPTURE and CHG-INBOX-EXPAND-PROPOSALS).

Outside the current product (observed: docs/_legacy/01_system/system-spec.md, Out of Scope for V1; docs/_legacy/01_system/system-overview.md, System Boundaries; docs/_legacy/09_roadmap/roadmap.md, Out of Scope for the Current Phase):

- finance, including any full finance platform;
- property management and inventory automation;
- documents as a standalone area;
- pets as a separate operational area (pets exist only within the household's people);
- automatic AI execution and advanced autonomous agents;
- external integrations beyond Phase 1 read-only Outlook import and read-only DomusMind calendar subscriptions, including calendar write-back and a large integration catalogue;
- external identity federation as a requirement, and complex multi-tenant concerns as a primary driver.

## Rationale

The roadmap builds a strong household operating core first; product clarity beats scope growth (observed: docs/_legacy/09_roadmap/roadmap.md, Purpose, Planning Rules 2). Calendar subscriptions extend the calendar sharing described in issue #16 while keeping both the feed and external access read-only.

## Consequences

- Outlook imports remain pull-only and do not write plans back to the provider (observed: CON-CALENDAR-OUTLOOK-PULL-ONLY).
- A calendar subscription is a separate, outbound read-only projection; it does not add another provider for importing external calendar entries (proposed by CHG-CALENDAR-SUBSCRIPTIONS).
- The excluded areas that the roadmap names as future (administration, documents, property and maintenance, inventory, integrations, intelligence) are modelled only as low-confidence roadmap drafts under CON-PRODUCT-PHASED-ROADMAP (decision Q-0004).
- Current product messaging must stay grounded in what exists now (observed: docs/_legacy/09_roadmap/roadmap.md, Planning Rule 4).
