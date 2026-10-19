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
  - TERM-FINANCIAL-COMMITMENT
  - TERM-EXPENSE
provenance:
  source: "docs/_legacy/01_system/system-spec.md (V1 Bounded Contexts, Out of Scope for V1); docs/_legacy/01_system/system-overview.md (Core System Model, System Boundaries); docs/_legacy/09_roadmap/roadmap.md (Out of Scope for the Current Phase); decision Q-0001; GitHub issues #16, #17, and #18"
  confidence: medium
  recovered-from: documentation
---
## Constraint

The current product covers the household and its people, areas of responsibility, plans with the Agenda and Phase 1 external calendars, read-only calendar subscriptions, dated family timeline notes, tasks and routines, shared lists, meal planning with recipes, a review-first Inbox for household notes and assistant action proposals, household Chats with an explicitly invoked assistant, and—proposed by this Product Change—manual household financial commitments, due-payment visibility, actual-expense records and basic spending summaries. Household finances are household-scoped; access to individual financial records may be narrower than general household access. This is coordination and historical cost visibility, not banking, accounting or financial advice.

Outside the current product:

- bank synchronization/Open Banking, account balances, transaction imports, reconciliation, automated payments, income/transfer ledger, investment management, tax preparation and double-entry accounting;
- advanced budgeting, forecasting, credit scoring or financial advice;
- property management and inventory automation (separate proposals may add their own scoped capabilities);
- documents as a standalone area (separately proposed in CHG-HOUSEHOLD-DOCUMENTS-001);
- pets as a separate operational area (pets exist only within the household's people);
- assistant-initiated or automatic execution of household changes without explicit Inbox approval;
- external integrations beyond Phase 1 read-only Outlook import and read-only DomusMind calendar subscriptions, including external messaging integration, calendar write-back and a large integration catalogue;
- external identity federation as a requirement, and complex multi-tenant concerns as a primary driver.

## Rationale

DomusMind supports shared household clarity. Finance should make agreed commitments and recorded costs visible without claiming to know bank balances or to reconcile accounts; household permissions remain explicit and contextual.

## Consequences

- Outlook imports remain pull-only and do not write plans back to the provider (CON-CALENDAR-OUTLOOK-PULL-ONLY).
- A calendar subscription is a separate, outbound read-only projection.
- Chat threads stay inside one household and expose private history only to their participants (CHG-HOUSEHOLD-CHATS).
- Assistant questions disclose the selected provider's data boundary; fallback identifies the target that handled the request (CHG-CONVERSATIONAL-ASSISTANT).
- No assistant-generated proposal changes a household record before explicit approval in Inbox.
- Finance does not import account activity or imply reconciliation; due values are expected and expenses are manually recorded actual facts.
- Current product messaging must stay grounded in what exists now; this draft only proposes future financial coordination.
