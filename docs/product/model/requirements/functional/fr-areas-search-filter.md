---
id: FR-AREAS-SEARCH-FILTER
type: functional-requirement
title: Search and filter Areas
status: draft
derived-from:
- UC-AREAS-REVIEW-OWNERSHIP
verification:
- scenario: Searching for a Support person's name lists the Areas that person supports
- scenario: Applying the unowned filter shows only Areas without an Owner
uses-terms:
- TERM-AREA
- TERM-OWNERSHIP-GAP
provenance:
  source: docs/_legacy/00_product/surfaces/areas.md (Data)
  confidence: medium
  recovered-from: documentation
---

## Requirement

The Areas surface MUST let a person search Areas by Area name, Owner name or Support name, and filter them by all, unowned, mine, active and archived. Filtering by a specific person is planned (future).

## Rationale

Finding an Area or one's own Areas quickly (observed: docs/_legacy/00_product/surfaces/areas.md, Data).
