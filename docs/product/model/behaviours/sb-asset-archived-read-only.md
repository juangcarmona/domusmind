---
id: SB-ASSET-ARCHIVED-READ-ONLY
type: structured-behaviour
title: An archived asset refuses ordinary editing
status: active
illustrates:
  - UC-ASSETS-REGISTER-AND-ORGANIZE
  - BR-ASSET-ARCHIVE-AND-RESTORE
given:
  - "a member is authorized for the household"
  - "the Asset is Archived"
when: "the member attempts to change its name through ordinary editing"
then:
  - "the edit is refused"
  - "the Asset remains Archived with its previous name and stored information"
uses-terms:
  - TERM-ASSET
---

## Intent

Keep the Archived Asset read-only until explicit restoration.

## Boundaries

This example states product outcomes; it is not evidence that an implementation or test has run. Other lifecycle actions and independent document access or obligation behavior retain their own rules.
