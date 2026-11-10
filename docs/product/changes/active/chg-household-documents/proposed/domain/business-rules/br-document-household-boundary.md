---
id: BR-DOCUMENT-HOUSEHOLD-BOUNDARY
type: business-rule
title: Every document belongs to exactly one household
status: draft
uses-terms:
  - TERM-DOCUMENT
  - TERM-HOUSEHOLD
---
## Rule

Every Document, file version, access grant and association belongs to one household. A Document cannot be associated with an entity in a different household, and a cross-household identifier never establishes authority.

## Rationale

Household is DomusMind's isolation boundary; document content can contain highly sensitive information.

## Examples

- A household member cannot attach a file to a member, Asset or Plan from another household.
- Moving or copying a Document between households is not an implicit consequence of changing household membership.

## Exceptions

No cross-household sharing is proposed in the MVP. Any future transfer must be an explicit product capability with separate consent and audit rules.
