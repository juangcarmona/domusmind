---
id: QR-DOCUMENTS-UPLOAD-SAFETY
type: quality-requirement
title: Reject unsafe or unbounded document uploads
status: draft
quality-attribute: security
applies-to:
  - BC-HOUSEHOLD-DOCUMENTS
  - UC-DOCUMENTS-UPLOAD-AND-DESCRIBE
verification:
  - scenario: "Server-side allowlist, decoded extension, detected media/signature, size/quota, generated filename and malware validation reject disallowed, mismatched, oversized, malicious or unscanned uploads before retrieval."
  - scenario: "Client-provided filename, MIME type and storage path cannot control the stored identity or executable location."
---
## Requirement

Uploads are authenticated and authorized, restricted to agreed formats and sizes, validated using server-observed content rather than client claims, stored under application-generated identities outside the web root, and withheld until required malware/content checks pass. File content is protected in transit and at rest by an explicitly owned, verified control; the deployment/storage choice and responsibility boundary remain architecture decisions. Preview is limited to explicitly safe formats; unknown or active content cannot execute in the DomusMind origin.

## Measurement

Automated tests cover format/signature mismatch, malformed files, size and quota limits, filename/path attacks, scanner unavailable/failure, interrupted uploads, content-serving headers, protected transport/storage configuration and unauthorized retrieval. Operational checks verify storage is non-executable and not publicly addressable.
