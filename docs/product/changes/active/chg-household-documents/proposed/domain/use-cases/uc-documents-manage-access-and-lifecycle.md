---
id: UC-DOCUMENTS-MANAGE-ACCESS-AND-LIFECYCLE
type: use-case
title: Change document access, version or lifecycle deliberately
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-DOCUMENTS
governed-by:
  - BR-DOCUMENT-ACCESS-INTERSECTION
  - BR-DOCUMENT-REPLACEMENT-PRESERVES-HISTORY
  - BR-DOCUMENT-EXPIRY-DOES-NOT-CREATE-WORK
uses-terms:
  - TERM-DOCUMENT
  - TERM-DOCUMENT-VERSION
  - TERM-DOCUMENT-ACCESS-POLICY
---
## Goal

Keep a Document current and correctly shared while retaining useful history and explicit lifecycle control.

## Trigger

A member needs to correct/replace a file, renew a policy, change permitted members, archive an outdated record or request deletion.

## Preconditions

The member is authenticated, authorized for the household and permitted to administer this Document under the accepted policy.

## Main Flow

1. Member chooses a distinct action: replace file, add renewed/superseding Document, change access, archive, restore or delete.
2. System confirms consequential access/removal operations and validates the member's authority.
3. System preserves the prior version or Document history as specified by policy.
4. System updates discoverability and associations consistently and records an auditable outcome.

## Alternative Flows

- A member restores an archived Document during its retention period.
- Deletion request is scheduled/soft-deleted pending the approved retention and backup policy.

## Failure Conditions

- The action is ambiguous (replacement versus renewal), retention/legal hold rules prevent deletion, or authorization is insufficient.

## Postconditions

The current accessible state is clear; prior information is retained or removed according to the accepted policy, not silently overwritten.
