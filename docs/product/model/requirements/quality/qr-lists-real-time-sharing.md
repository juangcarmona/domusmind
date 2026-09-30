---
id: QR-LISTS-REAL-TIME-SHARING
type: quality-requirement
title: "List changes are visible to the whole household without refresh"
status: draft
quality-attribute: "consistency"
applies-to:
  - BC-LISTS
verification:
  - scenario: "Two household members have the same list open; when one adds an item and checks another, the other member sees both changes without manually refreshing."
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Purpose); docs/_legacy/04_contexts/shared-lists.md (Responsibilities: real-time shared updates)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

List state MUST be shared in real time across household members: additions, toggles and updates made by one person MUST become visible to everyone viewing the list without a manual refresh (observed: openspec/specs/lists/spec.md, Purpose).

## Measurement

A change made by one person appears on another person's open view of the same list without any refresh action. The sources give no latency threshold (inferred: gap; a threshold must be agreed before this can be measured strictly).
