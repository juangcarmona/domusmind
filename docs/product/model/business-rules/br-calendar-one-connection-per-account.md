---
id: BR-CALENDAR-ONE-CONNECTION-PER-ACCOUNT
type: business-rule
title: "One active connection per person and provider account"
status: draft
applies-to:
  - "UC-CALENDAR-CONNECT-OUTLOOK"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection); docs/_legacy/04_contexts/calendar.md (External Calendar Connection Integrity); docs/_legacy/06_interfaces/external-calendar-api.md (Connect Outlook account, 409)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A person may have several external calendar connections, but not two active connections to the same provider account (observed: calendar spec; legacy calendar.md).

## Rationale

Duplicate connections would import every entry twice (inferred).

## Examples

- Ana connects her work Outlook, then tries again with the same account: rejected (observed: calendar spec).
- Ana connects her work and her personal Outlook accounts: allowed (inferred from "zero to many" connections).

## Exceptions

None.
