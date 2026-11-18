---
id: SB-ASSET-RESTORED
type: structured-behaviour
title: Restoring an archived asset returns the same asset to Active
status: active
illustrates:
  - UC-ASSETS-RESTORE
  - BR-ASSET-ARCHIVE-AND-RESTORE
given:
  - "an authorized household member can manage an Archived Asset"
  - "its stored information, relationships and recorded history are retained"
when: "the member explicitly restores it"
then:
  - "the same Asset is Active again in the same household"
  - "its identity, stored information, relationships and recorded history are preserved"
  - "ordinary authorized editing and use become available under their existing rules"
  - "restoration has not rewritten recorded historical outcomes"
uses-terms:
  - TERM-ASSET
covers-transition:
  lifecycle: LC-ASSET
  transition: RESTORE
---

## Intent

Restore the existing Asset rather than creating a replacement.

## Boundaries

This example states product outcomes; it is not evidence that an implementation or test has run. Other lifecycle actions and independent document access or obligation behavior retain their own rules.
