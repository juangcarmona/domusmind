---
id: FR-INBOX-INTERPRET
type: functional-requirement
title: Interpret a request as reviewable proposals
status: draft
derived-from:
  - UC-INBOX-CAPTURE-AND-REVIEW
verification:
  - scenario-ref: SB-INBOX-CAPTURE-AND-INTERPRET
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
  - TERM-ASSISTANT-PROVIDER-TARGET
  - TERM-LIST-ITEM
---
## Requirement

The product MAY interpret a captured text note or a supported household-thread action request and suggest structured proposals to create or update Calendar Plans, create Tasks, add or update List Items, add reminders to existing Plans, and create standalone dated family timeline notes. An action request from a Chat is eligible only when it comes from a household thread; a private-thread request is not copied into Inbox. The product MUST distinguish extracted information, unresolved ambiguity and proposed fields. If information needed by the owning area is missing or ambiguous, the assistant MUST ask a focused clarification or leave the required details for member refinement; it MUST NOT create a proposal that appears complete while required information is missing. A Reminder proposal MUST identify an existing Plan and a positive offset before approval. A Timeline Note proposal MUST include a date and note text before approval. A List Item addition proposal MUST identify an active list the member may edit and contain an item name. A List Item update proposal MUST identify the existing item and the fields to change, and the member MUST be authorized to edit that list. A Plan update proposal MUST identify an existing Plan the member may edit and the fields to change. Every proposal remains unapplied until an authorized member explicitly approves it. The configured provider route may be hosted or local; Inbox MUST disclose which target receives the text and authorized context and identify when fallback occurs.

## Rationale

Structured proposals reduce repeated entry while keeping existing area behavior authoritative and the member responsible for each household change.
