---
id: QR-DOCUMENTS-ACCESS-CONTROL
type: quality-requirement
title: Protect documents with household and document-specific authorization
status: draft
quality-attribute: security
applies-to:
  - BC-HOUSEHOLD-DOCUMENTS
  - UC-DOCUMENTS-UPLOAD-AND-DESCRIBE
  - UC-DOCUMENTS-FIND-AND-RETRIEVE
  - UC-DOCUMENTS-ASSOCIATE-WITH-CONTEXT
verification:
  - scenario: "Automated authorization tests prove that cross-household members, non-granted members, removed members and unauthorized contextual viewers cannot discover metadata, previews or file bytes through any API or surface route."
  - scenario: "Every denied/unknown policy path fails closed and audit records identify the action without recording document content or secrets."
uses-terms:
  - TERM-DOCUMENT-ACCESS-POLICY
---
## Requirement

All document operations enforce household isolation and explicit document audience policy at the point of access. Contextual associations cannot widen access. Restricted document metadata and content are protected equally; authorization is checked for each preview/download and is not replaced by an opaque identifier or long-lived public URL.

## Measurement

Cross-household, member-restricted, removed-member, child-document, guessed-identifier, stale-link and authorization-service failure tests pass for list, search, context surfaces, previews and downloads. Security review confirms deny-by-default behavior and privacy-safe audit logs.
