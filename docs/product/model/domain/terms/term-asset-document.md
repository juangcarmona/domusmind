---
id: TERM-ASSET-DOCUMENT
type: domain-term
title: Asset Document
status: active
defined-in: BC-HOUSEHOLD-ASSETS
---
## Definition

Metadata and an authorization-bound reference to paperwork relevant to an Asset, such as a warranty, invoice, policy, manual or inspection report.

## Distinguish From

Document metadata and association are distinct from binary file storage. A reference must not bypass the source file's access policy.

## Usage

The first release stores metadata and an authorized link/reference, not uploaded bytes. Its metadata is visible only to household members granted access explicitly to that document, subject also to the source content's access policy. An authorized member may remove an Asset Document association independently of the Asset and its retained history; this does not delete content held by the source storage provider. Binary upload and its storage, retention, deletion, backup, sensitive-data handling and content validation belong to a later separately approved capability.
