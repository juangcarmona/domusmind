---
id: UC-DOCUMENTS-ASSOCIATE-WITH-CONTEXT
type: use-case
title: Associate a document with a household context entity
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-DOCUMENTS
governed-by:
  - BR-DOCUMENT-ASSOCIATION-DOES-NOT-TRANSFER-MEANING
  - BR-DOCUMENT-ACCESS-INTERSECTION
uses-terms:
  - TERM-DOCUMENT
  - TERM-DOCUMENT-ASSOCIATION
---
## Goal

Make paperwork retrievable from the household context where it is useful without duplicating or changing that context's record.

## Trigger

A member wants to connect a Document to a Person or supported household entity such as an Asset, Plan or Task.

## Preconditions

The member may manage the Document and is authorized to reference the target. The target belongs to the same household and its context exposes a supported association contract.

## Main Flow

1. Member chooses a supported target type and resolves the intended entity.
2. System validates target identity and household scope through the target context contract.
3. System records the typed association without changing the target.
4. Authorized surfaces may navigate between the Document and target after independently checking access to both.

## Alternative Flows

- A member removes a stale association while retaining the Document and target.
- Asset target associations remain unavailable until the Household Assets and Documents ownership contracts are explicitly aligned.

## Failure Conditions

- Target context does not support the type, target is missing/cross-household, or authorization cannot be established.
- A link would reveal restricted document metadata to an unauthorized member.

## Postconditions

The association is discoverable only to members authorized for the necessary sides; context ownership remains unchanged.
