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
  - scenario: "Cross-household authorization and document-reference integration tests prove that unauthorized members cannot enumerate assets, obligations, history or file content."
---
## Requirement

Every asset operation and Agenda projection enforces the existing household/member authorization boundary. Sensitive document metadata and bytes/references are not exposed to a member unless that member is authorized by both the household and the source storage policy.

## Measurement

Automated tests cover cross-household identifiers, removed members, inaccessible document references and projection leakage; security review verifies that all read and write paths apply the same boundary.
