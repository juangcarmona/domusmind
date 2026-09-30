---
id: FR-WEB-APP-SHELL
type: functional-requirement
title: "One persistent shell for every surface"
status: draft
derived-from:
  - "UC-WEB-MOVE-BETWEEN-SURFACES"
  - "CON-PRODUCT-SEPARATE-AXES"
verification:
  - scenario-ref: "SB-WEB-NAVIGATE-AGENDA-TO-LISTS"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-LIST"
  - "TERM-AREA"
  - "TERM-MEAL-PLAN"
  - "TERM-TASK"
provenance:
  source: "openspec/specs/web-app/spec.md (Purpose, App Shell and Navigation, Notes); docs/_legacy/00_product/surface-system.md (Shell Model, Layout Grammar, Summary); interview: product owner decision Q-0051 (E-0158); interview: product owner decision Q-0052 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The web app MUST present one persistent shell on every surface: a navigation giving access to each primary surface with the current one marked, a compact page header, the main content area and an optional contextual detail panel. On mobile the navigation MUST collapse into a compact pattern, content MUST fill the screen and contextual detail MUST appear as a bottom sheet or pushed section (observed: openspec/specs/web-app/spec.md, App Shell and Navigation; docs/_legacy/00_product/surface-system.md, Shell Model). All surfaces MUST share the same layout, visual tone and interaction model so the product feels like one system (observed: openspec/specs/web-app/spec.md, Purpose).

The primary surfaces are Agenda, Lists, Areas, Meal Planning and Settings (observed: openspec/specs/web-app/spec.md, Purpose; decided: Q-0051). The web-app spec left Meal Planning out of the navigation pending its V1 status; Meal Planning is a navigable surface of the web app (decided: Q-0051). There is no dedicated Tasks surface: tasks live in the Agenda and in Areas (observed: openspec/specs/web-app/spec.md, Notes: Tasks surface; decided: Q-0052).

## Rationale

Households should not have to coordinate across separate tools or screens that feel unrelated (observed: web-app spec, Purpose).
