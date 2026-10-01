---
id: SB-LISTS-BROWSE-EXCLUDES-ARCHIVED
type: structured-behaviour
title: "An archived list is not among the active lists"
status: draft
illustrates:
  - UC-LISTS-BROWSE-LISTS
  - BR-LISTS-ARCHIVE-STATE
given:
  - "a list has been archived"
when: "a person asks for the household's active lists"
then:
  - "the archived list does not appear"
uses-terms:
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Retrieval; Scenario: Archived list is excluded)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Archiving removes a list from the default view.

## Boundaries

Does not say whether archived lists can be listed on request; the sources only say they are excluded by default.
