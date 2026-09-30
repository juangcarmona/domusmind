---
id: FR-WEB-CONTEXTUAL-DETAIL-PATTERNS
type: functional-requirement
title: "Inspect in place, confirm in a modal, navigate only for depth"
status: draft
derived-from:
  - "UC-AGENDA-INSPECT-ENTRY"
  - "UC-LISTS-OPEN-LIST"
  - "UC-AREAS-REVIEW-OWNERSHIP"
verification:
  - scenario: "On desktop, selecting a list item opens its detail in the side panel with the list still visible; deleting it asks for confirmation in a modal; on mobile the same detail opens as a bottom sheet (inferred from web-app spec, Inspector and Modal Usage)."
uses-terms:
  - "TERM-LIST-ITEM"
  - "TERM-AREA"
  - "TERM-AGENDA"
provenance:
  source: "openspec/specs/web-app/spec.md (Inspector and Modal Usage, Agenda Selection and Inspection, Lists Surface Structure, Areas Surface); docs/_legacy/00_product/surface-system.md (Contextual Detail, Surface Pattern Defaults, Anti-Patterns)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

Every surface MUST use the same detail patterns (observed: openspec/specs/web-app/spec.md, Inspector and Modal Usage; docs/_legacy/00_product/surface-system.md, Surface Pattern Defaults):

- Inspecting one selected item and light editing MUST happen in an inspector (side panel on desktop, bottom sheet on mobile) that keeps the surrounding content visible; this is the default for item detail everywhere.
- A modal MUST be used only for destructive actions needing confirmation and for short flows that must be completed or cancelled before continuing; it MUST NOT be the default for inspecting items.
- Full-page navigation MUST be used only when moving to a distinct work context or a flow needing depth; simple inspection MUST NOT navigate away.

## Rationale

Inspectors reduce navigation cost and keep context visible; modal chaos on desktop is an anti-pattern (observed: surface-system.md, Anti-Patterns, Success Criteria).
