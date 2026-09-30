---
id: SB-WEB-NAVIGATE-AGENDA-TO-LISTS
type: structured-behaviour
title: "Moving from the Agenda to Lists keeps the shell"
status: draft
illustrates:
  - "UC-WEB-MOVE-BETWEEN-SURFACES"
given:
  - "the person is on the Agenda"
when: "the person selects Lists in the navigation"
then:
  - "the Lists surface opens in the main content area"
  - "the navigation and page header remain visible and unchanged"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-LIST"
provenance:
  source: "openspec/specs/web-app/spec.md (App Shell and Navigation: Scenario User navigates from Agenda to Lists)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Changing surface never loses the product shell.

## Boundaries

Says nothing about which list is selected on arrival.
