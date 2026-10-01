---
id: FR-WEB-LOCAL-CAPTURE
type: functional-requirement
title: "Create things from where you are"
status: draft
derived-from:
  - "UC-AGENDA-CREATE-FROM-AGENDA"
  - "UC-LISTS-ADD-ITEM"
  - "UC-AREAS-CREATE-WORK-FROM-AREA"
  - "CON-PRODUCT-SEPARATE-AXES"
verification:
  - scenario: "A person on the Agenda adds a plan, a person in a list adds an item, and a person inspecting an Area adds a task for it; none of them is taken to a separate page (inferred from web-app spec, Inspector and Modal Usage)."
uses-terms:
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-LIST-ITEM"
  - "TERM-AREA"
provenance:
  source: "openspec/specs/web-app/spec.md (Inspector and Modal Usage, Mobile Behavior); docs/_legacy/00_product/surface-system.md (Interaction Grammar: Capture stays local)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

Contextual creation actions (add plan, add task, add routine, add list item) MUST stay close to the current surface and MUST NOT require navigating to a separate page; on mobile they MUST be reachable through a floating action where desktop uses a compact header action (observed: openspec/specs/web-app/spec.md, Inspector and Modal Usage, Mobile Behavior; docs/_legacy/00_product/surface-system.md, Interaction Grammar "Capture stays local").

## Rationale

Capture must stay easier than remembering (see QR-PRODUCT-CAPTURE-EASIER-THAN-REMEMBERING).
