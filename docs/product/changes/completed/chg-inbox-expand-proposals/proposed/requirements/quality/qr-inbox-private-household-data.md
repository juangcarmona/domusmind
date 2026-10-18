---
id: QR-INBOX-PRIVATE-HOUSEHOLD-DATA
type: quality-requirement
title: Protect raw Inbox notes and interpretations
status: draft
quality-attribute: security
applies-to:
  - UC-INBOX-CAPTURE-AND-REVIEW
verification:
  - scenario-ref: SB-INBOX-CAPTURE-AND-INTERPRET
  - scenario-ref: SB-INBOX-REVIEW-PROPOSAL
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Requirement

Raw Inbox text, interpretation results and proposals MUST be accessible only to authorized members of their household. The product MUST disclose whether captured text is sent outside the household's deployment for interpretation. The deployment selects the interpreter provider by configuration. A proposal MUST NOT grant access to information beyond the household member's existing permissions.

## Measurement

- An unrelated household cannot retrieve an Inbox item or its proposals.
- An unauthorized member cannot approve a proposal.
- The interpretation data flow matches the product's stated privacy boundary.
- The Inbox identifies when a configured interpreter sends text outside the deployment.
