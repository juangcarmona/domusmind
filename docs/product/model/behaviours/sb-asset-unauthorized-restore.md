---
id: SB-ASSET-UNAUTHORIZED-RESTORE
type: structured-behaviour
title: A member cannot restore an asset in another household
status: active
illustrates:
  - UC-ASSETS-RESTORE
  - BR-ASSET-ARCHIVE-AND-RESTORE
given:
  - "an Archived Asset belongs to another household"
  - "the member is not authorized for that household"
when: "the member attempts to restore that Asset"
then:
  - "the restore attempt is rejected"
  - "the Asset remains Archived and unchanged"
uses-terms:
  - TERM-ASSET
---

## Intent

Restoration does not bypass household authorization.

## Boundaries

This example states product outcomes; it is not evidence that an implementation or test has run. Other lifecycle actions and independent document access or obligation behavior retain their own rules.
