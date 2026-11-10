---
id: FR-ASSETS-REFERENCE-DOCUMENTS
type: functional-requirement
title: Associate authorized paperwork with an asset
status: draft
derived-from:
  - UC-ASSETS-REGISTER-AND-ORGANIZE
  - UC-ASSETS-COMPLETE-OBLIGATION
verification:
  - scenario: "An authorized member can find and separately remove an asset's document metadata and authorized link; an unauthorized member cannot view the metadata or source content; first-release upload is unavailable."
---
## Requirement

An authorized member can add, discover and separately remove document metadata and authorized links/references associated with an Asset. Metadata is visible only to household members explicitly granted access to that document; opening the reference additionally requires source-content authorization. Removing the association does not delete external source content or the Asset's read-only history. The first release does not upload or store binary files; a later upload capability requires a separate secure storage, access, retention, deletion, backup and content-validation decision.

## Rationale

Invoices, warranties, policies and service reports are useful only when findable without widening access to sensitive household information.
