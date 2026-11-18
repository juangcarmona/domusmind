---
id: SB-ASSET-REGISTERED-ACTIVE
type: structured-behaviour
title: A newly registered asset is Active
status: active
illustrates:
  - UC-ASSETS-REGISTER-AND-ORGANIZE
  - BR-ASSET-ARCHIVE-AND-RESTORE
given:
  - "a member is authorized to register Assets in the household"
  - "a valid asset name and category are provided"
when: "the member registers the Asset"
then:
  - "one household-scoped Asset exists with a stable identity"
  - "its lifecycle state is Active"
  - "ordinary authorized editing is available"
uses-terms:
  - TERM-ASSET
---

## Intent

Establish the initial Asset state without inventing a pre-existence lifecycle state.

## Boundaries

This example states product outcomes; it is not evidence that an implementation or test has run. Other lifecycle actions and independent document access or obligation behavior retain their own rules.
