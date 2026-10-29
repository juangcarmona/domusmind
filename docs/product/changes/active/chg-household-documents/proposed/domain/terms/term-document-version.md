---
id: TERM-DOCUMENT-VERSION
type: domain-term
title: Document Version
status: draft
defined-in: BC-HOUSEHOLD-DOCUMENTS
uses-terms:
  - TERM-DOCUMENT
---
## Definition

An immutable stored-file revision belonging to a logical Document, with its own content identity, size, detected media type and creation metadata.

## Distinguish From

A version is a replacement or correction of the file for the same logical document. A renewed policy or certificate with a new period may be a distinct Document that supersedes another, not merely a file version.

## Usage

Replacement preserves the previous version according to the accepted retention policy. The latest permitted version is the default for retrieval; authorized history may expose earlier versions where product policy allows.
