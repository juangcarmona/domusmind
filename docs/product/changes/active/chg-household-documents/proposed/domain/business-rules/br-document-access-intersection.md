---
id: BR-DOCUMENT-ACCESS-INTERSECTION
type: business-rule
title: Document access requires both household and document authorization
status: draft
uses-terms:
  - TERM-DOCUMENT
  - TERM-DOCUMENT-ACCESS-POLICY
---
## Rule

A requester may discover document metadata, preview or retrieve content only when authorized for the household and allowed by the Document's explicit access policy. If access depends on a contextual entity, its owning context must also authorize that disclosure. Missing, ambiguous, stale or failed authorization denies access. Association, household membership, uploader identity or manager role alone does not widen restricted access unless an accepted policy explicitly grants it.

## Rationale

Household paperwork can contain identity, school, financial, insurance and health-adjacent information. Broad household membership is not sufficient justification for exposing every file.

## Examples

- A member-restricted document does not appear in another member's search results or leak its title/date through an Asset inspector.
- A user authorized to see an Asset but not the linked Document cannot preview or retrieve that file.
- A signed download URL, if ever used, cannot outlive or bypass the authorization decision that issued it.

## Exceptions

No default administrative bypass is defined here. Product owners must decide manager recovery/access rules and child-document policy before acceptance.
