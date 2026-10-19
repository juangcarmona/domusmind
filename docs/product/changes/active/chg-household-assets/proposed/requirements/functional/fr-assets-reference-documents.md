---
id: FR-ASSETS-REFERENCE-DOCUMENTS
type: functional-requirement
title: Associate authorized paperwork with an asset
status: draft
derived-from:
  - UC-ASSETS-REGISTER-AND-ORGANIZE
  - UC-ASSETS-COMPLETE-OBLIGATION
verification:
  - scenario: "An authorized member can find an asset's document reference, while an unauthorized member cannot view its metadata or content; upload is unavailable until secure storage is approved."
---
## Requirement

An authorized member can discover document metadata and references associated with an Asset only when authorized to access the referenced content. File upload/storage is enabled only after a secure storage, access, retention, deletion, backup and content-validation decision; otherwise the MVP must clearly distinguish a reference from an uploaded DomusMind file.

## Rationale

Invoices, warranties, policies and service reports are useful only when findable without widening access to sensitive household information.
