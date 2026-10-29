---
id: FR-DOCUMENTS-METADATA-AND-ACCESS
type: functional-requirement
title: Describe documents and manage their permitted audience
status: draft
derived-from:
  - UC-DOCUMENTS-UPLOAD-AND-DESCRIBE
  - UC-DOCUMENTS-MANAGE-ACCESS-AND-LIFECYCLE
  - BR-DOCUMENT-ACCESS-INTERSECTION
verification:
  - scenario: "A member chooses a document audience and only household-authorized members allowed by that policy can discover title, category, dates, filename, preview or content."
  - scenario: "A document with missing, revoked or unresolved audience policy is denied by default."
uses-terms:
  - TERM-DOCUMENT
  - TERM-DOCUMENT-ACCESS-POLICY
---
## Requirement

Members can record useful descriptive metadata and set or change an explicit household-shared or member-restricted audience according to an accepted policy. Every library query, contextual projection, preview and download enforces household authorization and document policy; context-based access is additionally checked with the target owner. Restricted metadata must not leak through search, counts, errors, logs or linked surfaces.

## Rationale

Household documents can be more sensitive than the household's ordinary shared operational data. Document visibility therefore needs a policy narrower than household membership when appropriate.
