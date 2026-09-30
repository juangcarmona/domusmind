---
id: SB-CALENDAR-CONNECT-MISSING-ACCESS
type: structured-behaviour
title: "Connecting without the required access is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "BR-CALENDAR-OUTLOOK-REQUIRED-ACCESS"
given:
  - "a delegated authorization lacks calendar read or continued (offline) access"
when: "a member tries to connect the account"
then:
  - "the connection is rejected"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection: Connection with missing required scopes is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that both read and continued access are required.

## Boundaries

None.
