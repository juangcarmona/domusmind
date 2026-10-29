---
id: TERM-DOCUMENT-ASSOCIATION
type: domain-term
title: Document Association
status: draft
defined-in: BC-HOUSEHOLD-DOCUMENTS
uses-terms:
  - TERM-DOCUMENT
---
## Definition

A typed, household-scoped reference connecting a Document to an entity whose owning context gives that entity its meaning.

## Distinguish From

An association does not copy or own the target entity, prove a legal relationship, create work, or grant permission to the Document or target.

## Usage

The Documents context manages the link, while the target context validates the target identity and supplies authorized navigation/summary behavior. Access checks are enforced independently for the Document and target on every request.
