---
id: CON-PRODUCT-CURRENT-SCOPE
type: constraint
title: The product covers household coordination and connected household-managed things
status: draft
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-EXTERNAL-CALENDAR-ENTRY
  - TERM-CALENDAR-SUBSCRIPTION
  - TERM-ASSISTANT-PROVIDER-TARGET
  - TERM-ASSET
provenance:
  source: "Current accepted model; proposed scope extension in CHG-HOUSEHOLD-ASSETS-001"
  confidence: medium
  recovered-from: documentation
---
## Constraint

The current product covers the household and its people, areas of responsibility, plans with the Agenda and Phase 1 external calendars, read-only calendar subscriptions, dated family timeline notes, tasks and routines, shared lists, meal planning with recipes, a review-first Inbox for household notes and assistant action proposals, household Chats with an explicitly invoked assistant, and household-managed things with their associated obligations, documents/references and history. Chats answers read-only questions and may stage supported household action requests as Inbox proposals; every household change still requires explicit Inbox approval. Households configure one to three ordered hosted or local assistant provider targets, with visible fallback and data-sharing disclosures (CHG-CONVERSATIONAL-ASSISTANT).

Household Assets does not make DomusMind a real-estate/property-management, stock-inventory, finance/accounting, or standalone document-management product. It covers only documents, costs and responsibilities meaningfully associated with a household-managed thing.

Outside the current product:

- finance as a full finance platform, accounting, budgeting, banking or depreciation;
- property-management workflows, real-estate operations and stock/inventory automation;
- documents as a standalone area or generic document-management product;
- pets as a separate operational area (pets exist only within the household's people);
- assistant-initiated or automatic execution of household changes without explicit Inbox approval;
- external integrations beyond Phase 1 read-only Outlook import and read-only DomusMind calendar subscriptions, including external messaging integration, calendar write-back and a large integration catalogue;
- external identity federation as a requirement, and complex multi-tenant concerns as a primary driver.

## Rationale

The product supports shared household clarity. Asset records and persistent obligations connect managed household things to responsibility and history without duplicating the product areas that own time, execution, finance or file-storage infrastructure.

## Consequences

- Outlook imports remain pull-only and do not write plans back to the provider (CON-CALENDAR-OUTLOOK-PULL-ONLY).
- A calendar subscription is a separate, outbound read-only projection.
- Chat threads stay inside one household and expose private history only to their participants (CHG-HOUSEHOLD-CHATS).
- Assistant questions disclose the selected provider's data boundary; fallback identifies the target that handled the request (CHG-CONVERSATIONAL-ASSISTANT).
- No assistant-generated proposal changes a household record before explicit approval in Inbox.
- Current product messaging must stay grounded in what exists now; Household Assets claims remain future intent until its Product Change is accepted and delivered.
