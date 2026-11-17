---
id: SB-ASSET-UNAUTHORIZED-ARCHIVE
type: structured-behaviour
title: A member cannot archive an asset in another household
status: active
illustrates:
  - UC-ASSETS-ARCHIVE
  - BR-ASSET-ARCHIVE-AND-RESTORE
given:
  - "an Active Asset belongs to another household"
  - "the member is not authorized for that household"
when: "the member attempts to archive that Asset"
then:
  - "the archive attempt is rejected"
  - "the Asset remains Active and unchanged"
uses-terms:
  - TERM-ASSET
---

## Intent

Lifecycle changes preserve the existing household authorization boundary.

## Boundaries

This example states product outcomes; it is not evidence that an implementation or test has run. Other lifecycle actions and independent document access or obligation behavior retain their own rules.
