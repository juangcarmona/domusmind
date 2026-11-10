---
id: QR-ASSETS-HOUSEHOLD-PRIVACY
type: quality-requirement
title: Enforce household authorization for asset data and paperwork
status: draft
quality-attribute: "security"
applies-to:
  - BC-HOUSEHOLD-ASSETS
  - UC-ASSETS-REGISTER-AND-ORGANIZE
  - UC-ASSETS-REVIEW-HOUSEHOLD-ASSETS
verification:
  - scenario: "Cross-household authorization and document-reference integration tests prove that unauthorized members cannot enumerate assets, obligations or history; a household member without an explicit document grant cannot view its metadata, and source content retains its own access check."
---
## Requirement

Every asset operation and Agenda projection enforces the existing household/member authorization boundary. Asset Document metadata and references are visible only to explicitly granted household members; accessing the referenced source additionally obeys its own authorization policy. The first release does not handle uploaded document bytes.

## Measurement

Automated tests cover cross-household identifiers, removed members, inaccessible document references and projection leakage; security review verifies that all read and write paths apply the same boundary.
