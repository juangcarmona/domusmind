---
id: SB-CALENDAR-CONNECT-DUPLICATE-ACCOUNT
type: structured-behaviour
title: "Connecting the same Outlook account twice is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "BR-CALENDAR-ONE-CONNECTION-PER-ACCOUNT"
given:
  - "a member already has an active connection to an Outlook account"
when: "the member connects the same account again"
then:
  - "the connection is rejected"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection: Duplicate connection for the same account is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes one active connection per person and account.

## Boundaries

None.
