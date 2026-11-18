---
id: SB-ASSET-ARCHIVED-WITHOUT-HISTORY
type: structured-behaviour
title: Removing an asset without history archives it
status: active
illustrates:
  - UC-ASSETS-ARCHIVE
  - BR-ASSET-ARCHIVE-AND-RESTORE
given:
  - "an authorized household member can manage an Active Asset"
  - "the Asset has no recorded history"
when: "the member removes it through the ordinary action"
then:
  - "the same Asset is Archived and has not been permanently deleted"
  - "its stored information and relationships are retained"
  - "it remains available for authorized read-only consultation and explicit restoration"
uses-terms:
  - TERM-ASSET
covers-transition:
  lifecycle: LC-ASSET
  transition: ARCHIVE
---

## Intent

Ordinary removal preserves an Asset even before any history exists.

## Boundaries

This example states product outcomes; it is not evidence that an implementation or test has run. Other lifecycle actions and independent document access or obligation behavior retain their own rules.
