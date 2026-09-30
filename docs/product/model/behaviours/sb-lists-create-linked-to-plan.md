---
id: SB-LISTS-CREATE-LINKED-TO-PLAN
type: structured-behaviour
title: "Creating a list linked to a plan keeps list semantics"
status: draft
illustrates:
  - UC-LISTS-CREATE-LIST
  - BR-LISTS-ITEM-IS-NOT-A-TASK
given:
  - "a household exists"
when: "a person creates a list with a name and a linked plan"
then:
  - "the list is created and associated with the plan"
  - "the list's items are not converted to tasks or scheduled entries"
uses-terms:
  - TERM-LIST
  - TERM-PLAN
  - TERM-TASK
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Creation; Scenario: Household creates a list linked to a plan)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Linking a list to a plan is contextual; it does not change what the items are.

## Boundaries

None.
