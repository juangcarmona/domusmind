---
id: BR-LISTS-PRIVATE-LIST
type: business-rule
title: "A list can optionally be private to one person"
status: draft
applies-to:
  - UC-LISTS-MAKE-LIST-PRIVATE
  - BC-LISTS
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
  - TERM-AGENDA
provenance:
  source: "docs/_legacy/00_product/surfaces/lists.md (Purpose: shared by default, optionally private); interview: product owner decision Q-0027 (E-0158)"
  confidence: "medium"
  recovered-from: "interview"
---

## Rule

A list is shared with the household by default. A list can optionally be private to one person; a private list is not shared with the other people of the household (decided: Q-0027). The current code has no privacy or owner field on lists (observed: src/backend/DomusMind.Domain/Lists/SharedList.cs), so this is a product obligation the implementation does not yet meet.

Not settled by the decision or the sources (inferred: gap): who may make a list private and to whom, whether a private list can be made shared again, and whether dated items of a private list appear in the Agenda only in that person's member scope or not at all.

## Rationale

Some lists are personal (for example a personal gift idea list) while living in the same household product (observed: docs/_legacy/00_product/surfaces/lists.md, Purpose).

## Examples

- A person keeps a private list of gift ideas; the rest of the household does not see it.
- A new grocery list, created without choosing privacy, is shared with everyone.

## Exceptions

None stated.
