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

The product MAY interpret a captured note and suggest one or more structured proposals for Calendar Plans and Tasks. The product MUST distinguish extracted information, unresolved ambiguity and proposed fields. If information needed by the owning area is missing or ambiguous, the proposal MUST make that clear for member refinement.

## Rationale

Structured proposals can reduce repeated entry while keeping existing area behavior authoritative (issue #17; CON-PRODUCT-NO-MAGIC-AUTOMATION).
