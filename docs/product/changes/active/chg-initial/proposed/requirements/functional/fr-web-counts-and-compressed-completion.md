---
id: FR-WEB-COUNTS-AND-COMPRESSED-COMPLETION
type: functional-requirement
title: "Show relevance counts up front and compress what is done"
status: draft
derived-from:
  - "UC-LISTS-BROWSE-LISTS"
  - "UC-LISTS-OPEN-LIST"
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-AREAS-REVIEW-OWNERSHIP"
verification:
  - scenario: "The list switcher shows each list's unchecked count, the Agenda month cell shows entry counts, and an Area shows its open task count, all before any detail is opened; checked items in a list sit under a collapsed \"Completed (N)\" and completed Agenda items stay visible but de-emphasised (observed: web-app spec, Lists Surface Structure, List Lifecycle, Agenda Time Modes, Areas Surface)."
uses-terms:
  - "TERM-LIST"
  - "TERM-AGENDA"
  - "TERM-AREA"
provenance:
  source: "docs/_legacy/00_product/surface-system.md (Interaction Grammar); openspec/specs/web-app/spec.md (Lists Surface Structure, List Lifecycle, Agenda Time Modes, Agenda Item Priority Ordering, Areas Surface)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

Surfaces MUST expose relevance through counts before any detail is opened (for example unchecked list items, items today, open tasks, upcoming items), and completed or checked items MUST remain accessible but MUST NOT dominate the default view (observed: docs/_legacy/00_product/surface-system.md, Interaction Grammar "Counts should be visible", "Completed state should compress"; corroborated by openspec/specs/web-app/spec.md, Lists Surface Structure, List Lifecycle, Agenda Item Priority Ordering, Areas Surface).

## Rationale

People decide where to look next from state they can see at a glance (inferred from surface-system.md, Main Content).
