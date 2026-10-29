---
id: UC-DOCUMENTS-UPLOAD-AND-DESCRIBE
type: use-case
title: Add a household document with useful details
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-DOCUMENTS
governed-by:
  - BR-DOCUMENT-HOUSEHOLD-BOUNDARY
  - BR-DOCUMENT-ACCESS-INTERSECTION
uses-terms:
  - TERM-DOCUMENT
  - TERM-DOCUMENT-ACCESS-POLICY
---
## Goal

Preserve important household paperwork in a secure, findable place with an intentional audience.

## Trigger

A member receives, scans or locates an invoice, policy, warranty, certificate, contract, manual or other household document.

## Preconditions

The member is authorized for the household. The file type, size and storage service are allowed by the accepted security policy.

## Main Flow

1. Member selects a file and supplies a clear title and category.
2. Member may enter issuer, issue/expiry dates, reference number and optional simple tags.
3. Member explicitly selects household-shared or a restricted member audience according to the accepted policy.
4. System validates the upload server-side, stores it under an application-generated identity, and withholds retrieval until required validation/scanning succeeds.
5. System records the Document and initial immutable file version, scoped to the household.
6. Member may associate it with an authorized household member or supported context entity.

## Alternative Flows

- Member stores a metadata-only external reference only if an approved product policy supports it; a reference is not represented as a managed uploaded file.
- Member adds a restricted document for a child; only explicitly permitted members can discover it, subject to the unresolved household policy.

## Failure Conditions

- The requester lacks household authorization or the chosen audience is invalid.
- File type/signature/size is disallowed, scanning fails or is unavailable under a fail-closed policy, storage fails, or the target association is invalid.

## Postconditions

An authorized audience can find the Document and retrieve only a validated, permitted version; unauthorized members cannot infer its existence from metadata.
