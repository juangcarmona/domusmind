---
id: FR-LISTS-PRIVATE-LIST
type: functional-requirement
title: "Lists are shared by default and can be private to one person"
status: draft
derived-from:
  - UC-LISTS-MAKE-LIST-PRIVATE
  - BR-LISTS-PRIVATE-LIST
verification:
  - scenario: "A list created without a privacy choice is visible to every person in the household."
  - scenario: "A list made private to one person is not visible to the other people of the household."
uses-terms:
  - TERM-LIST
provenance:
  source: "docs/_legacy/00_product/surfaces/lists.md (Purpose: shared by default, optionally private); interview: product owner decision Q-0027 (E-0158)"
  confidence: "medium"
  recovered-from: "interview"
---

## Requirement

The product MUST share every list with the whole household by default. The product MUST let a list be made private to one person, after which it MUST NOT be shared with the other people of the household (decided: Q-0027). The Agenda treatment of a private list's dated items is not yet decided (inferred: gap; see BR-LISTS-PRIVATE-LIST).

## Rationale

Households share most lists, but some lists are personal.
