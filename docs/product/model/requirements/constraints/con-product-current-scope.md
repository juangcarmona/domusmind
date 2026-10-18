---
id: CON-PRODUCT-CURRENT-SCOPE
type: constraint
title: The current product covers household coordination and its supporting areas
status: draft
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-EXTERNAL-CALENDAR-ENTRY
  - TERM-CALENDAR-SUBSCRIPTION
  - TERM-ASSISTANT-PROVIDER-TARGET
provenance:
  source: "docs/_legacy/01_system/system-spec.md (V1 Bounded Contexts, Out of Scope for V1); docs/_legacy/01_system/system-overview.md (Core System Model, System Boundaries); docs/_legacy/09_roadmap/roadmap.md (Out of Scope for the Current Phase); decision Q-0001; GitHub issues #16, #17, and #18"
  confidence: medium
  recovered-from: documentation
---
## Constraint

The current product covers the household and its people, areas of responsibility, plans with the Agenda and Phase 1 external calendars, read-only calendar subscriptions, dated family timeline notes, tasks and routines, shared lists, meal planning with recipes, a review-first Inbox for household notes and assistant action proposals, and household Chats with an explicitly invoked assistant. Chats answers read-only questions and may stage supported household action requests as Inbox proposals; supported initial actions create or update Calendar Plans and add or update List Items. Every household change still requires explicit Inbox approval. Households configure one to three ordered hosted or local assistant provider targets, with visible fallback and data-sharing disclosures (CHG-CONVERSATIONAL-ASSISTANT).

Outside the current product:

- finance, including any full finance platform;
- property management and inventory automation;
- documents as a standalone area;
- pets as a separate operational area (pets exist only within the household's people);
- assistant-initiated or automatic execution of household changes without explicit Inbox approval;
- external integrations beyond Phase 1 read-only Outlook import and read-only DomusMind calendar subscriptions, including external messaging integration, calendar write-back and a large integration catalogue;
- external identity federation as a requirement, and complex multi-tenant concerns as a primary driver.

## Rationale

The product supports shared household clarity. The conversational assistant can answer from authorized information and stage reviewed proposals without bypassing product-area ownership or household permissions.

## Consequences

- Outlook imports remain pull-only and do not write plans back to the provider (CON-CALENDAR-OUTLOOK-PULL-ONLY).
- A calendar subscription is a separate, outbound read-only projection.
- Chat threads stay inside one household and expose private history only to their participants (CHG-HOUSEHOLD-CHATS).
- Assistant questions disclose the selected provider's data boundary; fallback identifies the target that handled the request (CHG-CONVERSATIONAL-ASSISTANT).
- No assistant-generated proposal changes a household record before explicit approval in Inbox.
- Current product messaging must stay grounded in what exists now.
