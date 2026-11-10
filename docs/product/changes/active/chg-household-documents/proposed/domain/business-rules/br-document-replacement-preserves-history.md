---
id: BR-DOCUMENT-REPLACEMENT-PRESERVES-HISTORY
type: business-rule
title: Replacing a managed file does not silently erase its prior version
status: draft
uses-terms:
  - TERM-DOCUMENT
  - TERM-DOCUMENT-VERSION
---
## Rule

Replacing the file for the same logical Document creates a new immutable version and retains the previous version subject to the accepted retention policy. A renewed or separately issued document with a distinct validity period should be represented as a new Document that can identify the prior Document it supersedes. Archive and delete are separate lifecycle actions; neither is an implicit file replacement.

## Rationale

Households may need prior insurance periods, corrected invoices or warranty evidence. Destructive overwrite makes those facts unavailable and obscures what changed.

## Examples

- Re-uploading a clearer scan of the same invoice creates a replacement version.
- A renewed car policy is a new record linked as superseding the expired policy rather than erasing its coverage dates.

## Exceptions

Permanent deletion, retention periods, recoverable trash and version access policy remain open product/security decisions.
