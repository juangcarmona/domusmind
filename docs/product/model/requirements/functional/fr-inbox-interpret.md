---
id: FR-INBOX-INTERPRET
type: functional-requirement
title: Interpret an Inbox note as reviewable proposals
status: draft
derived-from:
  - UC-INBOX-CAPTURE-AND-REVIEW
verification:
  - scenario-ref: SB-INBOX-CAPTURE-AND-INTERPRET
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Requirement

The product MAY interpret a captured text note and suggest structured proposals for Calendar Plans, Tasks, reminders attached to existing Plans, and standalone dated family timeline notes. The product MUST distinguish extracted information, unresolved ambiguity and proposed fields. If information needed by the owning area is missing or ambiguous, the proposal MUST make that clear for member refinement. A Reminder proposal MUST identify an existing Plan and a positive offset before approval. A Timeline Note proposal MUST include a date and note text before approval. The configured interpreter may be local or outside the household deployment; Inbox MUST disclose when note text leaves that deployment.

## Rationale

Structured proposals can reduce repeated entry while keeping existing area behavior authoritative (issue #17; CON-PRODUCT-NO-MAGIC-AUTOMATION).
