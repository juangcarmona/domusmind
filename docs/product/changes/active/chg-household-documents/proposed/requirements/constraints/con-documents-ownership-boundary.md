---
id: CON-DOCUMENTS-OWNERSHIP-BOUNDARY
type: constraint
title: Documents owns the file record, not the meaning of linked entities
status: draft
applies-to:
  - BC-HOUSEHOLD-DOCUMENTS
uses-terms:
  - TERM-DOCUMENT
  - TERM-DOCUMENT-ASSOCIATION
---
## Constraint

Household Documents owns managed document identity, metadata, file versions, access policy, lifecycle and typed association links. The owning context of each target retains entity identity, domain meaning, lifecycle and authorization. Document expiry is descriptive unless the appropriate domain explicitly creates a separate action. Household access is the outer security boundary; document-specific restrictions and target-context permissions may narrow access but never widen it.

## Rationale

This creates a reliable shared file capability without duplicating Household, Assets, Calendar, Tasks or future Finance/School domain behavior, and protects member-restricted paperwork.

## Consequences

- Assets must link to Document identities instead of owning file bytes or duplicating document metadata; its business meaning for warranties and asset paperwork remains authoritative.
- Calendar and Tasks may expose authorized document links without transferring Plan or Task ownership.
- No absent Finance or School context is introduced as a dependency.
- No expiry-triggered Task, Plan or generic Obligation is created automatically.
- Storage, encryption, scanning, backup and deletion implementation choices require architecture decisions backed by the self-hosted runtime and operational model.
