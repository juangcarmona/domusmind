---
id: CON-PRODUCT-CURRENT-SCOPE
type: constraint
title: The current product covers household coordination and its supporting areas
status: draft
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-EXTERNAL-CALENDAR-ENTRY
  - TERM-CALENDAR-SUBSCRIPTION
provenance:
  source: "docs/_legacy/01_system/system-spec.md (V1 Bounded Contexts, Out of Scope for V1); docs/_legacy/01_system/system-overview.md (Core System Model, System Boundaries); docs/_legacy/09_roadmap/roadmap.md (Out of Scope for the Current Phase); decision Q-0001; GitHub issues #16, #17, and #18"
  confidence: medium
  recovered-from: documentation
---

## Constraint

The current product covers the household and its people, areas of responsibility, plans with the Agenda and Phase 1 external calendars, read-only calendar subscriptions, dated family timeline notes, tasks and routines, shared lists, meal planning with recipes, a review-first Inbox for household notes that can propose Plans, Tasks, Plan reminders and timeline notes, and household Chats with an explicitly invoked read-only assistant (observed: docs/_legacy/01_system/system-spec.md, V1 Bounded Contexts; docs/_legacy/01_system/system-overview.md, Core System Model; meal planning is current product by decision Q-0001; calendar subscriptions are proposed by CHG-CALENDAR-SUBSCRIPTIONS; Inbox by CHG-INBOX-ASSISTED-CAPTURE and CHG-INBOX-EXPAND-PROPOSALS; Chats by CHG-HOUSEHOLD-CHATS).

Outside the current product (observed: docs/_legacy/01_system/system-spec.md, Out of Scope for V1; docs/_legacy/01_system/system-overview.md, System Boundaries; docs/_legacy/09_roadmap/roadmap.md, Out of Scope for the Current Phase):

- finance, including any full finance platform;
- property management and inventory automation;
- documents as a standalone area;
- pets as a separate operational area (pets exist only within the household's people);
- automatic AI execution and advanced autonomous agents;
- external integrations beyond Phase 1 read-only Outlook import and read-only DomusMind calendar subscriptions, including external messaging integration, calendar write-back and a large integration catalogue;
- external identity federation as a requirement, and complex multi-tenant concerns as a primary driver.

## Rationale

The product supports shared household clarity. Chats adds a household-scoped way for members to coordinate and ask read-only questions without replacing existing general-purpose messaging or bypassing the product's existing record ownership.

## Consequences

- Outlook imports remain pull-only and do not write plans back to the provider (observed: CON-CALENDAR-OUTLOOK-PULL-ONLY).
- A calendar subscription is a separate, outbound read-only projection; it does not add another provider for importing external calendar entries (proposed by CHG-CALENDAR-SUBSCRIPTIONS).
- Chat threads stay inside one household and expose private history only to their participants (proposed by CHG-HOUSEHOLD-CHATS).
- Chats assistant answers are read-only; members use Inbox to review proposed household changes (proposed by CHG-HOUSEHOLD-CHATS).
- The excluded areas that the roadmap names as future (administration, documents, property and maintenance, inventory, integrations, intelligence) are modelled only as low-confidence roadmap drafts under CON-PRODUCT-PHASED-ROADMAP (decision Q-0004).
- Current product messaging must stay grounded in what exists now (observed: docs/_legacy/09_roadmap/roadmap.md, Planning Rule 4).
