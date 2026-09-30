---
id: SB-LISTS-CLEAR-PLAN-LINK
type: structured-behaviour
title: "Clearing a list's plan link leaves its items untouched"
status: draft
illustrates:
  - UC-LISTS-UPDATE-LIST
  - BR-LISTS-CONTEXT-LINKS-INFORMATIONAL
given:
  - "a list is linked to a plan"
when: "a person clears the list's plan link"
then:
  - "the list is no longer linked to the plan"
  - "the list's items are unaffected"
uses-terms:
  - TERM-LIST
  - TERM-PLAN
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Update; Scenario: Household removes a plan link)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Links can be explicitly cleared and carry no consequences for items.

## Boundaries

None.
