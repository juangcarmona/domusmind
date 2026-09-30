---
id: UC-WEB-MOVE-BETWEEN-SURFACES
type: use-case
title: "Move between the product's surfaces"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
governed-by: []
uses-terms:
  - "TERM-AGENDA"
  - "TERM-LIST"
  - "TERM-AREA"
provenance:
  source: "openspec/specs/web-app/spec.md (Purpose, App Shell and Navigation, Notes: Meal Planning surface); docs/_legacy/00_product/surface-system.md (Shell Model); docs/_legacy/00_product/surfaces/settings.md (Entry Points); interview: product owner decision Q-0051 (E-0158); interview: product owner decision Q-0052 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The person reaches any primary surface (Agenda, Lists, Areas, Meal Planning and Settings; decided: Q-0051; there is no dedicated Tasks surface, decided: Q-0052) from one consistent shell and always knows where they are (observed: openspec/specs/web-app/spec.md, App Shell and Navigation).

## Trigger

The person wants to work on another part of household life.

## Preconditions

The person is signed in to their household.

## Main Flow

1. The person selects a surface in the navigation.
2. DomusMind opens that surface in the main content area.
3. The navigation and page header stay in place and the navigation marks the current surface.

## Alternative Flows

- On mobile the navigation is a compact pattern (drawer or bottom strip) and the surface fills the screen (observed: web-app spec, App Shell and Navigation).
- A contextual link, such as "Open in Lists" from the Agenda or a link to Settings when calendar setup is missing, opens the target surface with the relevant item selected (observed: web-app spec, Agenda Projected List Items; settings.md, Entry Points).

## Failure Conditions

None known.

## Postconditions

The selected surface is shown inside the same shell.
