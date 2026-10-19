---
id: BR-DOCUMENT-ASSOCIATION-DOES-NOT-TRANSFER-MEANING
type: business-rule
title: Document associations preserve target-context ownership
status: draft
uses-terms:
  - TERM-DOCUMENT
  - TERM-DOCUMENT-ASSOCIATION
---
## Rule

Documents owns the Document and its association link. The target bounded context remains the source of truth for target identity, business meaning, lifecycle and authorization. Creating or removing a link does not create, update or delete the target. The link alone grants no access to either side.

## Rationale

An invoice, warranty or school form is meaningful through its domain context; Documents must not become a parallel store of Assets, Members, Plans or Tasks.

## Examples

- Household Documents stores a link to a boiler Asset; Household Assets owns what the Asset and warranty mean.
- A document linked to a Calendar Plan does not modify or reschedule the Plan.
- A document linked to a Person does not modify the household roster or that Person's profile.

## Exceptions

No association to an absent Finance or School context is supported until that context and its contract exist and are authorized.
