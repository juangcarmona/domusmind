---
id: SB-ASSET-ARCHIVED-WITH-HISTORY
type: structured-behaviour
title: Archiving an asset preserves its history
status: active
illustrates:
  - UC-ASSETS-ARCHIVE
  - BR-ASSET-ARCHIVE-AND-RESTORE
given:
  - "an authorized household member can manage an Active Asset"
  - "the Asset has recorded service history"
when: "the member removes it through the ordinary action"
then:
  - "the same Asset is Archived"
  - "its identity, stored information, relationships and recorded history remain intact"
  - "the history remains available to authorized members without ordinary editing"
uses-terms:
  - TERM-ASSET
covers-transition:
  lifecycle: LC-ASSET
  transition: ARCHIVE
---

## Intent

Putting an Asset away does not remove its household memory.

## Boundaries

This example states product outcomes; it is not evidence that an implementation or test has run. Other lifecycle actions and independent document access or obligation behavior retain their own rules.
