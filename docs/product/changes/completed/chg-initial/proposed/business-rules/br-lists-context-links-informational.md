---
id: BR-LISTS-CONTEXT-LINKS-INFORMATIONAL
type: business-rule
title: "Area and plan links on a list are optional and informational"
status: draft
applies-to:
  - UC-LISTS-CREATE-LIST
  - UC-LISTS-UPDATE-LIST
uses-terms:
  - TERM-LIST
  - TERM-AREA
  - TERM-PLAN
provenance:
  source: "openspec/specs/lists/spec.md (Purpose, List Creation, List Update); docs/_legacy/00_product/surfaces/lists.md (Creation Model, Relationship with Areas, Anti-Patterns); docs/_legacy/04_contexts/shared-lists.md (Boundaries: Responsibilities)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A list may be associated with an Area and linked to a Plan, at creation or later, and either link may be cleared. Links are never required, are contextual anchors only, do not affect ownership or responsibility, and do not change list semantics (observed: openspec/specs/lists/spec.md, Purpose, List Update; docs/_legacy/00_product/surfaces/lists.md, Creation Model).

## Rationale

Context enriches but does not define the list; forcing linkage is rejected as model drift (observed: docs/_legacy/00_product/surfaces/lists.md, Core Principles, Anti-Patterns).

## Examples

- A "School supplies" list associated with the School area appears as contextual memory in that area without changing who owns the area.
- Clearing a list's plan link leaves its items unaffected.

## Exceptions

None.
