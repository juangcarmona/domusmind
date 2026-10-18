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

Raw Inbox text, interpretation results and proposals MUST be accessible only to authorized members of their household. The product MUST disclose which configured provider target receives request text and authorized context, whether that target is hosted or local, and whether fallback occurred. A proposal MUST NOT grant access to information beyond the household member's existing permissions. Credentials MUST remain secret and MUST NOT be retrievable by ordinary household members. A request from a private Chat MUST NOT be copied into Inbox.

## Measurement

- An unrelated household cannot retrieve an Inbox item or its proposals.
- An unauthorized member cannot approve a proposal.
- The interpretation data flow matches the product's stated provider and privacy boundary.
- The Inbox identifies the target that handled an interpretation and whether a fallback occurred.
- Provider credentials are not returned in clear text after saving.
