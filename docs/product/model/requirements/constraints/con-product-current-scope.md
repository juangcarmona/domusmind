---
id: CON-PRODUCT-CURRENT-SCOPE
type: constraint
title: DomusMind coordinates household people, work, time and managed things
status: active
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-EXTERNAL-CALENDAR-ENTRY
  - TERM-CALENDAR-SUBSCRIPTION
  - TERM-ASSISTANT-PROVIDER-TARGET
  - TERM-ASSET
provenance:
  source: Current accepted model; proposed scope extension in CHG-HOUSEHOLD-ASSETS-001
  confidence: medium
  recovered-from: documentation
---

## Constraint

The current product covers:

- the household, its people and Areas of responsibility;
- Plans, the Agenda, Phase 1 external calendars, read-only calendar subscriptions and dated family timeline notes;
- Tasks, Routines and shared Lists;
- meal planning and recipes;
- Inbox notes and action proposals that members review;
- household Chats and an assistant that members invoke explicitly;
- household-managed things and their obligations, documents, references and history.

 Chats answers read-only questions and may stage supported household action requests as Inbox proposals; supported initial actions create or update Calendar Plans and add or update List Items. Every household change still requires explicit Inbox approval. Households configure one to three ordered hosted or local assistant provider targets, with visible fallback and data-sharing disclosures (CHG-CONVERSATIONAL-ASSISTANT).

Household Assets covers documents, costs and responsibilities associated with a household-managed thing. Its scope excludes real-estate and property-management operations, stock inventory, finance and accounting, and standalone document management.

Outside the current product:

- finance as a full finance platform, accounting, budgeting, banking or depreciation;
- property-management workflows, real-estate operations and stock/inventory automation;
- documents as a standalone area or generic document-management product;
- pets as a separate operational area (pets exist only within the household's people);
- assistant-initiated or automatic execution of household changes without explicit Inbox approval;
- external integrations beyond Phase 1 read-only Outlook import and read-only DomusMind calendar subscriptions, including external messaging integration, calendar write-back and a large integration catalogue;
- complex multi-tenant concerns as a primary driver.

## Rationale

The product supports shared household clarity. The conversational assistant can answer from authorized information and stage reviewed proposals without bypassing product-area ownership or household permissions. Asset records and persistent obligations connect managed household things to responsibility and history without duplicating the product areas that own time, execution, finance or file-storage infrastructure.

## Consequences

- Outlook imports remain pull-only and do not write plans back to the provider (CON-CALENDAR-OUTLOOK-PULL-ONLY).
- A calendar subscription is a separate, outbound read-only projection.
- Chat threads stay inside one household and expose private history only to their participants (CHG-HOUSEHOLD-CHATS).
- Assistant questions disclose the selected provider's data boundary; fallback identifies the target that handled the request (CHG-CONVERSATIONAL-ASSISTANT).
- No assistant-generated proposal changes a household record before explicit approval in Inbox.
- Production sign-in uses the operator-configured OIDC provider (CON-PRODUCTION-OIDC-SIGN-IN).
- Current product messaging must stay grounded in what exists now; Household Assets claims remain future intent until delivered.
